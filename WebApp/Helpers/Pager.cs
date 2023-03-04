using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Helpers
{
    public class Pager
    {
        public int TotalItems { get; set; } // umumi mehsul sayisi 

        public int CurrentPage { get; set; } // hansi sehhifede olduggumuzu gosterir 
        public int PageSize { get; set; } // nece dene gorsenecek onun ucun 
        public int TotalPage { get; set; } // umumi necedene sehife var
        public int StartPage { get; set; } // ilk sehifem 
        public int EndPage { get; set; } //  son sehhife 

        public Pager()
        {

        }

        public Pager(int totlItems, int page, int pageSize = 10)
        {
            int totalPage = (int)Math.Ceiling((decimal)totlItems  / (decimal)pageSize);
            int currentPage = page; 
            int startPage = currentPage - 5; 
            int endPage = currentPage + 4;
            if (startPage <=  0 )
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;
            }
            if (endPage > totalPage)
            {
                endPage = totalPage;
                if (endPage > 10)
                {
                    startPage = endPage - 9 ;
                }
                TotalItems = totlItems;
                CurrentPage = currentPage;
                PageSize = pageSize;
                StartPage = startPage ;
                EndPage = endPage;
                TotalPage = totalPage;

            }
        }
    }
}