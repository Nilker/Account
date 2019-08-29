using System;
using System.Collections.Generic;
using System.Text;

namespace Cig.YanFa.Account.Application.Dto
{
    public class PagedResultDto<T> : ListResultDto<T>
    {
        public PagedResultDto() { }

        public PagedResultDto(int totalCount, IReadOnlyList<T> items)
        {
            TotalCount = totalCount;
            Items = items;
        }
       
        public int TotalCount { get; set; }
    }


    public class ListResultDto<T> : IListResult<T>
    {
        public ListResultDto(){}
       
        public ListResultDto(IReadOnlyList<T> items)
        {
            Items = items;
        }
        public IReadOnlyList<T> Items { get; set; }
    }

    
    public interface IListResult<T>
    {
        IReadOnlyList<T> Items { get; set; }
    }
}
