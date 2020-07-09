using System;
using System.Collections.Generic;
using System.Text;

namespace OpenYS.NetCore
{
    
    public class MCamera
    {
        /// <summary>
        /// 设备序列号
        /// </summary>
        public string DeviceSerial { get; set; }

        /// <summary>
        /// 通道号
        /// </summary>
        public int ChannelNo { get; set; }

        /// <summary>
        /// 通道名
        /// </summary>
        public string ChannelName { get; set; }

        /// <summary>
        /// 在线状态：0-不在线，1-在线
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 图片地址（大图），若在萤石客户端设置封面则返回封面图片，未设置则返回默认图片
        /// </summary>
        public string PicUrl { get; set; }

        /// <summary>
        /// 是否加密，0：不加密，1：加密
        /// </summary>
        public int IsEncrypt { get; set; }

        /// <summary>
        /// 视频质量：0-流畅，1-均衡，2-高清，3-超清
        /// </summary>
        public int VideoLevel { get; set; }

        /// <summary>
        /// 分享设备的权限字段
        /// </summary>
        public int Permission { get; set; }
    }
}
