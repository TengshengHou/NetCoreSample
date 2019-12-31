namespace Project.Api.Dto
{
    public class UserIdentity
    {

        public int UserId { get; set; }
        public string Name { get; set; }
        //公司
        public string Company { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        public string Title { get; set; }
        //头像地址
        public string Avatar { get; set; }



    }
}
