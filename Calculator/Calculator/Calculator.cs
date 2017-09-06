using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Calculator : Form
    {

        public double? totalnum = null;
        public string tempnum = "";
        public string lastaction;
        public bool dot = false;
        
        
        public Calculator()
        {
            InitializeComponent();
        }

        private void Calculator_Load(object sender, EventArgs e)
        {

        }

        private void one_btn_Click(object sender, EventArgs e)
        {
            tempnum = tempnum + "1";
            dispaly.Text = tempnum;
        }

        private void two_btn_Click(object sender, EventArgs e)
        {
            tempnum = tempnum + "2";
            dispaly.Text = tempnum;
        }

        private void thr_btn_Click(object sender, EventArgs e)
        {
            tempnum = tempnum + "3";
            dispaly.Text = tempnum;
        }

        private void four_btn_Click(object sender, EventArgs e)
        {
            tempnum = tempnum + "4";
            dispaly.Text = tempnum;
        }

        private void five_btn_Click(object sender, EventArgs e)
        {
            tempnum = tempnum + "5";
            dispaly.Text = tempnum;
        }

        private void six_btn_Click(object sender, EventArgs e)
        {
            tempnum = tempnum + "6";
            dispaly.Text = tempnum;
        }

        private void sev_btn_Click(object sender, EventArgs e)
        {
            tempnum = tempnum + "7";
            dispaly.Text = tempnum;
        }

        private void eig_btn_Click(object sender, EventArgs e)
        {
            tempnum = tempnum + "8";
            dispaly.Text = tempnum;
        }

        private void nine_btn_Click(object sender, EventArgs e)
        {
            tempnum = tempnum + "9";
            dispaly.Text = tempnum;
        }

        private void eq_btn_Click(object sender, EventArgs e)
        {
            try
            {
                switch (lastaction)
                {
                    case "add":
                        totalnum = totalnum + Convert.ToDouble(tempnum);
                        break;

                    case "substract":
                        totalnum = totalnum - Convert.ToDouble(tempnum);
                        break;

                    case "multiply":
                        totalnum = totalnum * Convert.ToDouble(tempnum);
                        break;

                    case "divide":
                        totalnum = totalnum / Convert.ToDouble(tempnum);
                        break;

                    default:
                        totalnum = Convert.ToDouble(tempnum);
                        break;
                }
            }
            catch (System.FormatException) { }


    
        tempnum = Convert.ToString(totalnum); ;
        dispaly.Text = tempnum;
        dot = false;
        }

        private void add_btn_Click(object sender, EventArgs e)
        {
            if (tempnum != "")
            {
                if (totalnum == null)
                {
                    totalnum = 0;
                }
                totalnum = totalnum + Convert.ToDouble(tempnum);
            }
                tempnum = "";
                dispaly.Text = Convert.ToString(totalnum);
                lastaction = "add";
                dot = false;


        }

        private void sub_btn_Click(object sender, EventArgs e)
        {
            if (tempnum != "")
            {
                if (totalnum == null)
                {
                    totalnum = 0;
                }
                totalnum = totalnum - Convert.ToDouble(tempnum);
            }
                tempnum = "";
                dispaly.Text = Convert.ToString(totalnum);
                lastaction = "substract";
                dot = false;

        }

        private void zreo_btn_Click(object sender, EventArgs e)
        {
            if (dispaly.Text!="0")
            {

                tempnum = tempnum + "0";
                dispaly.Text = tempnum;

            }      
        }

        private void dot_btn_Click(object sender, EventArgs e)
        {
            if (dot == false & tempnum != "")
            {
                tempnum = tempnum + ",";
                dot = true;
            }
        }

        private void mult_btn_Click(object sender, EventArgs e)
        {
            if (tempnum != "")
            {
                if (totalnum == null)
                {
                    totalnum = 1;
                }
                totalnum = totalnum * Convert.ToDouble(tempnum);
            }
                
                tempnum = "";
                dispaly.Text = Convert.ToString(totalnum);
                lastaction = "multiply";
                dot = false;

        }

        private void div_btn_Click(object sender, EventArgs e)
        {
            if (tempnum != "" & tempnum != "0")
            {
                if (totalnum == null)
                {
                    totalnum = Convert.ToDouble(tempnum);
                }
                else
                {
                    totalnum = totalnum / Convert.ToDouble(tempnum);
                }
            }
                tempnum = "";
                dispaly.Text = Convert.ToString(totalnum);
                lastaction = "divide";
                dot = false;

        }

        private void opposition_Click(object sender, EventArgs e)
        {
            if (tempnum != "0")
            {
                double oppositiontmp = 1 / Convert.ToDouble(tempnum);
                tempnum = Convert.ToString(oppositiontmp);
                dispaly.Text = tempnum;
            }
            
        }

        private void reverse_Click(object sender, EventArgs e)
        {
            double reversed = -1 * Convert.ToDouble(tempnum);
            tempnum = Convert.ToString(reversed);
            dispaly.Text = tempnum;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int tempn_len = tempnum.Length;
            if (tempn_len >= 1)
            {
                tempnum = tempnum.Remove(tempn_len - 1); ;
                dispaly.Text = tempnum;
            }
        }

        private void CE_btn_Click(object sender, EventArgs e)
        {
            tempnum = "";
            dispaly.Text = tempnum;
        }

        private void C_btn_Click(object sender, EventArgs e)
        {
            totalnum = null;
            tempnum = "";
            dispaly.Text = tempnum;
        }

        private void sqrt_btn_Click(object sender, EventArgs e)
        {
            double tempn_sqrt=Math.Sqrt(Convert.ToDouble(tempnum));
            tempnum = Convert.ToString(tempn_sqrt);
            dispaly.Text = tempnum;
        }

        private void pow2_btn_Click(object sender, EventArgs e)
        {
            double tempn_pow2 = Math.Pow(Convert.ToDouble(tempnum), 2);
            tempnum = Convert.ToString(tempn_pow2);
            dispaly.Text = tempnum;
        }
    }
}
