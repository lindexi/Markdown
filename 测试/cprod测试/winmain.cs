using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace cprod测试
{
    [TestClass]
    public class winmain
    {
        [TestMethod]
        public void clipboard_substitution()
        {
            var view = new produproperty.ViewModel.winmain(null);
            string text = "要替换文本";
            //把替换两个字替换为string
            view.text = text;
            view.select = 1;
            view.select_length = 2;
            //view.clipboard_substitution("string");

            Assert.AreEqual("要string文本", view.text);

            view.text = text;
            view.select = 1;
            view.select_length = 5;
            //view.clipboard_substitution("string");
            Assert.AreEqual("要string", view.text);
        }

        [TestMethod]
        public void text_line()
        {
            //var view = new produproperty.ViewModel.winmain(null);
            //string text = "#readme";
            //Assert.AreEqual(text, view.text_line(text, 0));
            //string str = text;
            //text += "\nline";
            //Assert.AreEqual(str, view.text_line(text, 2));
            //str = "line";
            //int i;
            //i = text.IndexOf('\n') + 1;
            //Assert.AreEqual(str, view.text_line(text, i));
            //Assert.AreEqual("", view.text_line(text, 100));

        }
    }
}
