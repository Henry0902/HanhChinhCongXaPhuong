using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DNC.WEB.Common
{
    public class CommonFunction
    {
        public static bool DeleteFile(string paths, int type)
        {
            try
            {
                DirectoryInfo directory = new DirectoryInfo(paths);
                switch (type)
                {
                    case 1://1.xóa toàn bộ file theo đường dẫn thư mục
                        foreach (FileInfo file in directory.GetFiles())
                        {
                            file.Delete();
                        }
                        break;
                    case 2://2. xóa toàn bộ thư mục con thuộc đường dẫn
                        foreach (DirectoryInfo dir in directory.GetDirectories())
                        {
                            dir.Delete(true);
                        }
                        break;
                    case 3://3.file theo đường dẫn(đường dẫn bao gồm cả tên)
                        File.Delete(paths);
                        break;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}