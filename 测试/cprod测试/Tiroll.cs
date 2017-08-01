using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace cprod测试
{
    [TestClass]
    public class Tiroll
    {
        [TestMethod]
        public async Task Guidanceroll()
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///1.md"));
            Debug.Write(await FileIO.ReadTextAsync(file));
            var t = new produproperty.Tiroll();
            //t.Guidanceroll(await FileIO.ReadTextAsync(file));
        }
    }
}