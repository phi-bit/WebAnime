using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAnime.Models;  

namespace WebAnime.Models
{
    public class Giohang
    {
        dbwebmohinhDataContext data = new dbwebmohinhDataContext();
        public int iMamohinh { set; get; }
        public string sTenmohinh { set; get; }
        public string sAnhbia { set; get; }
        public Double dDongia { set; get; }
        public int iSoluong { set; get; }
        public Double dThanhtien
        {
            get { return iSoluong * dDongia; }

        }
        public Giohang(int Mamohinh)
        {
            iMamohinh = Mamohinh;
            MoHinh mohinh = data.MoHinhs.Single(n => n.Mamohinh == iMamohinh);
            sTenmohinh = mohinh.Tenmohinh;
            sAnhbia = mohinh.Anhbia;
            dDongia = double.Parse(mohinh.Giaban.ToString());
            iSoluong = 1;
        }
    }
}