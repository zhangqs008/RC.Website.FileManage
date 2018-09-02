using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using RC.FileManageClient.localhost;

namespace RC.FileManageClient
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (txtFilepath.Text.Length == 0)
            {
                MessageBox.Show("抱歉，请选择要上传的文件！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtAppKey.Text.Length <= 0 || txtAppSecret.Text.Length <= 0)
            {
                MessageBox.Show("抱歉，请填写Appkey和AppSecret！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var url = txtUrl.Text.Trim();
                var appkey = txtAppKey.Text.Trim();
                var appsecret = txtAppSecret.Text.Trim();
                var filepath = txtFilepath.Text.Trim();
                var tableName = txtTableName.Text.Trim();
                var tableKeyId = txtTableKeyId.Text.Trim();
                var thread = new Thread(() =>
                {
                    var sw = new Stopwatch();
                    sw.Start();
                    UpdateUI(() =>
                    {
                        btnUpload.Enabled = false;
                        lblStatus.Text = "处理中，请稍候...";
                    });
                    var res = Uploadfile(url, appkey, appsecret, filepath, tableName, tableKeyId);
                    sw.Stop();
                    var ts = sw.Elapsed;
                    var elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds,
                        ts.Milliseconds / 10);
                    UpdateUI(() =>
                    {
                        btnUpload.Enabled = true;
                        txtResult.Text = res;
                        lblStatus.Text = "请求完毕，耗时：" + elapsedTime;
                    });
                }) { IsBackground = true };
                thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("抱歉，上传异常：" + ex.Message, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void UpdateUI(Action action)
        {
            if (InvokeRequired)
                Invoke(action);
            else
                action();
        }

        /// <summary>
        ///     上传文件
        /// </summary>
        /// <param name="url">接口路径</param>
        /// <param name="appkey">由系统颁发</param>
        /// <param name="appsecret">由系统颁发</param>
        /// <param name="filepath">上传文件全路径</param>
        /// <param name="tableName">业务关联表</param>
        /// <param name="tableKeyId">业务表主键</param>
        /// <returns></returns>
        protected string Uploadfile(string url, string appkey, string appsecret, string filepath, string tableName,
            string tableKeyId)
        {
            var service = new UploadFile(); //webservice引用
            service.Url = url;
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var file = new FileInfo(txtFilepath.Text.Trim());
            var bytes = new byte[file.Length];
            using (var stream = file.OpenRead())
            {
                stream.Read(bytes, 0, Convert.ToInt32(file.Length));
            }

            var dic = new Dictionary<string, string> { { "appkey", appkey }, { "timestamp", timestamp } };
            var sign = SignRequest(dic, appsecret);
            var result = service.Upload(appkey, timestamp, sign, bytes, file.Name, tableName, tableKeyId);
            return result;
        }

        /// <summary>
        ///     接口签名算法
        /// </summary>
        /// <param name="parameters">请求参数</param>
        /// <param name="secret">由成都人才市场信息部颁发</param>
        /// <returns></returns>
        public static string SignRequest(IDictionary<string, string> parameters, string secret)
        {
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams =
                new SortedDictionary<string, string>(parameters, StringComparer.Ordinal);
            var dem = sortedParams.GetEnumerator();

            // 第二步：把所有参数名和参数值串在一起
            var query = new StringBuilder();

            while (dem.MoveNext())
            {
                var key = dem.Current.Key;
                var value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value)) query.Append(key).Append(value);
            }

            // 第三步：使用MD5/HMAC加密
            byte[] bytes;
            query.Append(secret);
            var md5 = MD5.Create();
            bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(query.ToString()));


            // 第四步：把二进制转化为大写的十六进制
            var result = new StringBuilder();
            for (var i = 0; i < bytes.Length; i++)
                result.Append(bytes[i].ToString("X2"));

            return result.ToString();
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Title = "打开(Open)";
            ofd.FileName = "";
            ofd.InitialDirectory =
                Environment.SpecialFolder.Desktop
                    .ToString(); //为了获取特定的系统文件夹，可以使用System.Environment类的静态方法GetFolderPath()。该方法接受一个Environment.SpecialFolder枚举，其中可以定义要返回路径的哪个系统目录
            ofd.Filter = "All(*.*)|*.*|文本文件(*.txt)|*.txt|cs文件(*.cs)|*.cs";
            ofd.ValidateNames = true; //文件有效性验证ValidateNames，验证用户输入是否是一个有效的Windows文件名
            ofd.CheckFileExists = true; //验证路径有效性
            ofd.CheckPathExists = true; //验证文件有效性
            if (ofd.ShowDialog() == DialogResult.OK) txtFilepath.Text = ofd.FileName;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var url = txtUrl.Text.Trim();
            url = url.Replace("UploadFile.asmx", "File/Preview/");
            if (url.Length > 0)
            {
                Help.ShowHelp(this, url);
            }
        }
    }
}