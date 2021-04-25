namespace UUMS.Application.Dtos
{
    public class ClientParam
    {
        /// <summary>
        /// 客户端名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否拥有菜单
        /// </summary>
        public bool HasMenu { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        public int SortNo { get; set; }
    }
}
