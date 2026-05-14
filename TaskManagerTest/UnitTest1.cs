using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using FlaUI.UIA3;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;

namespace TaskManagerTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestTaskManager()
        {
            Process.Start("mspaint.exe");
            Thread.Sleep(5000);

            Assert.IsTrue(IsProcessRunning("mspaint"), "Paint not running");

            Process.Start("taskmgr.exe");
            Thread.Sleep(5000);

            using (var automation = new UIA3Automation())
            {
                var desktop = automation.GetDesktop();
                var taskManagerProcess = Process.GetProcessesByName("Taskmgr").First();
                var taskManager = automation.FromHandle(taskManagerProcess.MainWindowHandle).AsWindow();

                taskManager.Focus();

                Thread.Sleep(1000);

                Keyboard.TypeSimultaneously(VirtualKeyShort.CONTROL, VirtualKeyShort.KEY_F);

                Thread.Sleep(1000);

                Keyboard.Type("paint");
                Thread.Sleep(5000);

                Keyboard.Press(VirtualKeyShort.DOWN);
                Thread.Sleep(5000);

                Keyboard.Press(VirtualKeyShort.LMENU);
                Keyboard.Press(VirtualKeyShort.KEY_E);
                Keyboard.Release(VirtualKeyShort.LMENU);
                Keyboard.Release(VirtualKeyShort.KEY_E);

                Thread.Sleep(3000);
                Assert.IsFalse(IsProcessRunning("mspaint"), "Paint is not killed");

                taskManager.Close();
            }
        }


        static bool IsProcessRunning(string name)
        {
            return Process.GetProcessesByName(name).Any();
        }
    }
}
