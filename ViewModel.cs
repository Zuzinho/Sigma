using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sigma.Models
{
    public class Paginations
    {
        public static void show_pagination(int pages_count,int current_page){
        bool arrow_left_active = current_page>1;
        bool has_left_ellipsis = current_page > 4;
        bool arrow_right_active = current_page < pages_count;
        bool has_right_ellipsis = (pages_count - current_page) > 3;
        int middle_section_start = 0;
        int middle_section_end = 0;
        if(has_left_ellipsis){
            middle_section_start = current_page - 2;
        }
        else{
            middle_section_start = 2;
        }
        if(has_right_ellipsis){
            middle_section_end = current_page + 3;
        }
        else{
            middle_section_end = pages_count;
        }
        
        if(arrow_left_active){
            Console.Write("< ");
        }
        else{
            Console.Write(" % ");
        }
        
        if(pages_count<8){
            for(int page = 1;page <= pages_count;page++){
                if(page!=current_page){
                    Console.Write(" {0} ",page);
                }
                else{
                    Console.Write(" <{0}> ",page);
                }
            }
        }
        else{
            if(current_page!=1){
                Console.Write(" 1 ");
            }
            else{
                Console.Write(" <1> ");
            }
            if(has_left_ellipsis){
                Console.Write("...");
            }
            for(int page = middle_section_start;page<middle_section_end;page++){
                if(page!=current_page){
                    Console.Write(" {0} ",page);
                }
                else{
                    Console.Write(" <{0}> ",page);
                }
            }
            if(has_right_ellipsis){
                Console.Write("...");
            }
            if(pages_count!=current_page){
                Console.Write(" {0} ",pages_count);
            }
            else{
                Console.Write(" <{0}> ",pages_count);
            }
        }
        if(arrow_right_active){
            Console.Write(" >");
        }
        else{
            Console.Write(" % ");
        }
        Console.Write('\n');
    }
    }
}
