using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZwellTech;

namespace ManaMagic.Logging
{
    public sealed class RichTextBoxWriter : TextWriter
    {
        private readonly RichTextBox windowControl = null;

        #region Properties

        public bool Enabled { get; set; } = false;

        public override Encoding Encoding
        {
            get { return Encoding.Unicode; }
        }

        #endregion

        #region Constructor

        private RichTextBoxWriter() { }

        public RichTextBoxWriter(RichTextBox textbox)
        {
            ValidationHelper.ThrowIfArgumentNull(textbox);

            this.windowControl = textbox;
            this.Enabled = true;
        }

        #endregion

        #region Public Method Overrides: Write

        public override void Write(double value)
        {
            this.Write(value.ToString());
        }

        public override void Write(char value)
        {
            this.Write(value.ToString());
        }

        public override void Write(string value)
        {
            this.WriteCore(value, false);
        }

        #endregion

        #region Public Method Overrides: WriteLine

        public override void WriteLine(double value)
        {
            this.WriteLine(value.ToString());
        }

        public override void WriteLine(char value)
        {
            this.WriteLine(value.ToString());
        }

        public override void WriteLine(string value)
        {
            this.WriteCore(value, true);
        }

        #endregion

        #region Public Method Overrides: WriteAsync

        public override Task WriteAsync(char value)
        {
            return this.WriteAsync(value.ToString());
        }

        public override Task WriteAsync(string value)
        {
            return this.WriteCoreAsync(value, false);
        }

        #endregion

        #region Public Method Overrides: WriteLineAsync

        public override Task WriteLineAsync(char value)
        {
            return this.WriteLineAsync(value.ToString());
        }

        public override Task WriteLineAsync(string value)
        {
            return this.WriteCoreAsync(value, true);
        }

        #endregion

        #region Private Methods: Util

        private void WriteCore(string value, bool appendNewLine)
        {
            this.windowControl.InvokeIfRequired(() =>
            {
                this.windowControl.AppendText(value);
                if (appendNewLine)
                {
                    this.windowControl.AppendText(Environment.NewLine);
                }
                this.ScrollToEnd();
            });
        }

        private Task WriteCoreAsync(string value, bool appendNewLine)
        {
            TaskCompletionSource<VoidType> taskCompletionSource = new TaskCompletionSource<VoidType>();
            try
            {
                this.windowControl.InvokeIfRequired(() =>
                {
                    this.windowControl.AppendText(value);
                    if (appendNewLine)
                    {
                        this.windowControl.AppendText(Environment.NewLine);
                    }
                    this.ScrollToEnd();

                    taskCompletionSource.SetResult(VoidType.Value);
                });
            }
            catch (Exception ex)
            {
                taskCompletionSource.SetException(ex);
            }

            return taskCompletionSource.Task;
        }

        private void ScrollToEnd()
        {
            this.windowControl.SelectionStart = this.windowControl.Text.Length;
            this.windowControl.ScrollToCaret();
        }

        #endregion
    }

    public static class ExtensionMethods
    {
        public static void InvokeIfRequired(this ISynchronizeInvoke invoker, MethodInvoker action)
        {
            if (invoker.InvokeRequired) { invoker.Invoke(action, null); }
            else { action(); }
        }
    }
}
