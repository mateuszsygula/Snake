using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Snake
{
    public partial class Ilosc_porazek : Component
    {
        public int max_porazek
        {
            get
            {
                return (int)max_porazek - 1;
            }
            set
            {
                if (value <= 0)
                {
               //     progressBar1 = value;
                }
                else
                {
                    
                }
            }
        }
        public Ilosc_porazek()
        {
            InitializeComponent();
        }

        public Ilosc_porazek(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
