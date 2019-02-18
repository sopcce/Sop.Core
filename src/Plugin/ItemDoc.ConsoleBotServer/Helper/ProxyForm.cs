using System;
using System.Windows.Forms;

namespace ItemDoc.ConsoleBotServer.Helper
{

    public partial class ProxyForm : Form
    {
        public ProxyForm()
        {
            InitializeComponent();
        }

        #region 无边框窗体拖动API

        /// <summary>
        ///     重写WndProc方法,实现窗体移动和禁止双击最大化
        /// </summary>
        /// <param name="m">Windows 消息</param>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x4e:
                case 0xd:
                case 0xe:
                case 0x14:
                    base.WndProc(ref m);
                    break;

                case 0x84: //鼠标点任意位置后可以拖动窗体
                    DefWndProc(ref m);
                    if (m.Result.ToInt32() == 0x01)
                    {
                        m.Result = new IntPtr(0x02);
                    }
                    break;

                case 0xA3: //禁止双击最大化
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        #endregion


        private void ProxyForm_Load(object sender, EventArgs e)
        {
           
        }
    }
}
 