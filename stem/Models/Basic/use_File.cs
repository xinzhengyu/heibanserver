using System;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Dhand.Basic
{
    public class use_File
    {
        /// <summary>
        /// 流转化图片
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Image image_base64(byte[] image)
        {
            Stream stream = new MemoryStream(image);
            Image iSource = Image.FromStream(stream);
            return iSource;
        }
        //保存图片的方法
        /// <summary>
        /// 保存流格式图片
        /// </summary>
        /// <param name="image"></param>
        /// <param name="path"></param>
        /// <param name="id"></param>
        /// <param name="is_thumbnail"></param>
        /// <param name="quality"></param>
        /// <param name="dWidth"></param>
        /// <param name="dHeight"></param>
        /// <returns></returns>
        public static string save_image_base64(byte[] image, string path, long id, bool is_thumbnail, int quality = 100, int dWidth = 0, int dHeight = 0)
        {
            //文件夹名字
            string _path = path;
            string Type = ".png";
            string imagename = Guid.NewGuid().ToString() + Type;
            //少一个判断图片类型是否符合规范

            //判断结束

            try
            {
                path = HttpRuntime.AppDomainAppPath.ToString() + "static\\" + path + "\\" + "dhand" + id + "\\";
                if (!System.IO.Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                };
                path = Path.Combine(path + imagename);
                Stream stream = new MemoryStream(image);
                Image iSource = Image.FromStream(stream);
                ImageFormat tFormat = iSource.RawFormat;
                int sW = 0, sH = 0;
                //按比例缩放
                Size tem_size = new Size(iSource.Width, iSource.Height);
                if (dWidth == 0 || dHeight == 0)
                {
                    dWidth = tem_size.Width;
                    dHeight = tem_size.Height;
                };
                if (tem_size.Width > dHeight || tem_size.Width > dWidth) //将**改成c#中的或者操作符号
                {

                    if ((tem_size.Width * dHeight) > (tem_size.Height * dWidth))
                    {

                        sW = dWidth;

                        sH = (dWidth * tem_size.Height) / tem_size.Width;

                    }

                    else
                    {

                        sH = dHeight;

                        sW = (tem_size.Width * dHeight) / tem_size.Height;

                    }

                }

                else
                {

                    sW = tem_size.Width;

                    sH = tem_size.Height;

                }

                Bitmap ob = new Bitmap(dWidth, dHeight);

                Graphics g = Graphics.FromImage(ob);

                g.Clear(Color.WhiteSmoke);

                g.CompositingQuality = CompositingQuality.HighQuality;

                g.SmoothingMode = SmoothingMode.HighQuality;

                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);

                g.Dispose();

                //以下代码为保存图片时，设置压缩质量

                EncoderParameters ep = new EncoderParameters();

                long[] qy = new long[1];

                qy[0] = quality;//设置压缩的比例1-100

                EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);

                ep.Param[0] = eParam;

                try
                {

                    ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();

                    ImageCodecInfo jpegICIinfo = null;

                    for (int x = 0; x < arrayICI.Length; x++)
                    {

                        if (arrayICI[x].FormatDescription.Equals("JPEG"))
                        {

                            jpegICIinfo = arrayICI[x];

                            break;

                        }

                    }

                    if (jpegICIinfo != null)
                    {

                        ob.Save(path, jpegICIinfo, ep);//dFile是压缩后的新路径

                    }

                    else
                    {

                        ob.Save(path, tFormat);

                    }

                    return Config.Ip + "/static/" + _path + "/" + "dhand" + id + "/" + imagename;

                }

                catch
                {

                    return "false";

                }

                finally
                {

                    iSource.Dispose();

                    ob.Dispose();

                }
            }
            catch
            {
                return "false";
            }
        }
        //参数分别为图片流，保存路径（格式为"image"），所需存储的id，图片是否需要压缩，可选：图片质量（默认为100，最大为100），图片宽度，图片高度
        public static string save_image(HttpPostedFileBase image, string path, int id, bool is_thumbnail, int quality = 100, int dWidth = 0, int dHeight = 0)
        {
            //文件夹名字
            string _path = path;
            string Type = System.IO.Path.GetExtension(image.FileName).ToLower();
            string imagename = Guid.NewGuid().ToString() + Type;
            //少一个判断图片类型是否符合规范

            //判断结束

            try
            {
                path = HttpRuntime.AppDomainAppPath.ToString() + "static\\" + path + "\\" + "dhand" + id + "\\";
                if (!System.IO.Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                };
                path = Path.Combine(path + imagename);
                Image iSource = Image.FromStream(image.InputStream);
                ImageFormat tFormat = iSource.RawFormat;
                int sW = 0, sH = 0;
                //按比例缩放
                Size tem_size = new Size(iSource.Width, iSource.Height);
                if (dWidth == 0 || dHeight == 0)
                {
                    dWidth = tem_size.Width;
                    dHeight = tem_size.Height;
                };
                if (tem_size.Width > dHeight || tem_size.Width > dWidth) //将**改成c#中的或者操作符号
                {

                    if ((tem_size.Width * dHeight) > (tem_size.Height * dWidth))
                    {

                        sW = dWidth;

                        sH = (dWidth * tem_size.Height) / tem_size.Width;

                    }

                    else
                    {

                        sH = dHeight;

                        sW = (tem_size.Width * dHeight) / tem_size.Height;

                    }

                }

                else
                {

                    sW = tem_size.Width;

                    sH = tem_size.Height;

                }

                Bitmap ob = new Bitmap(dWidth, dHeight);

                Graphics g = Graphics.FromImage(ob);

                g.Clear(Color.WhiteSmoke);

                g.CompositingQuality = CompositingQuality.HighQuality;

                g.SmoothingMode = SmoothingMode.HighQuality;

                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);

                g.Dispose();

                //以下代码为保存图片时，设置压缩质量

                EncoderParameters ep = new EncoderParameters();

                long[] qy = new long[1];

                qy[0] = quality;//设置压缩的比例1-100

                EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);

                ep.Param[0] = eParam;

                try
                {

                    ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();

                    ImageCodecInfo jpegICIinfo = null;

                    for (int x = 0; x < arrayICI.Length; x++)
                    {

                        if (arrayICI[x].FormatDescription.Equals("JPEG"))
                        {

                            jpegICIinfo = arrayICI[x];

                            break;

                        }

                    }

                    if (jpegICIinfo != null)
                    {

                        ob.Save(path, jpegICIinfo, ep);//dFile是压缩后的新路径

                    }

                    else
                    {

                        ob.Save(path, tFormat);

                    }

                    return Config.Ip + "/static/" + _path + "/" + "dhand" + id + "/" + imagename;

                }

                catch
                {

                    return "false";

                }

                finally
                {

                    iSource.Dispose();

                    ob.Dispose();

                }
            }
            catch
            {
                return "false";
            }
        }
        //传入参数分别为文件，文件夹名字，存入id的名字，传入文件需要符合的类型，如.jpg
        public static string save_file(HttpPostedFileBase file, string path, int id, string[] type = null)
        {
            //文件夹名字
            string _path = path;
            string Type = System.IO.Path.GetExtension(file.FileName).ToLower();
            string filename = Guid.NewGuid().ToString() + Type;
            //判断文件类型是否符合规范
            if (type != null)
            {
                if (!type.Contains(Type))
                {
                    return "false";
                }
            }
            //判断结束
            //尝试写入路径
            try
            {
                //存储路径
                path = HttpRuntime.AppDomainAppPath.ToString() + "static\\" + path + "\\" + "dhand" + id + "\\";
                //判断文件夹是否存在
                if (!System.IO.Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                };

                path = Path.Combine(path + filename);
                //保存文件
                file.SaveAs(path);
            }
            catch
            {
                return "false";
            }
            //成功后返回保存文件的路径
            return Config.Ip + "/static/" + _path + "/" + "dhand" + id + "/" + filename;
        }
        //删除文件的方法,传入文件储存数据库的位置
        public static bool delete_file(string path)
        {
            path = path.Replace("/", "\\");
            path = path.Substring(1, path.Length - 1);
            path = HttpRuntime.AppDomainAppPath.ToString() + path;

            if (!File.Exists(path))
            {
                return false;
            }
            File.Delete(@path);
            return true;
        }
    }
}