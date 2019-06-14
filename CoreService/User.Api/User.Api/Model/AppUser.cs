using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Api.Model
{
    public class AppUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //公司
        public string Company { get; set; }
        //职位
        public string Title { get; set; }
        //手机
        public string Phone{ get; set; }
        //头像地址
        public string Avatar { get; set; }
        //性别1：男 0：女
        public byte Gender { get; set; }
        //地址
        public string Address{ get; set; }

        public string Email { get; set; }

        public string Tel { get; set; }
        /// <summary>
        /// 省份ID
        /// </summary>
        public string ProvinceId { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }

        public int CityId { get; set; }
        /// <summary>
        /// //城市名称
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 用户属性列表
        /// </summary>
        public List<UserProperty> Properties { get;set }


    }
}
