using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GPOVizeOdev_ArabaList
{
    public partial class FrmCar : Form
    {
        public FrmCar()
        {
            InitializeComponent();
        }

        public Car car = null;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            propertyGrid1.SelectedObject= car;
        }

        private void OkClicked(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void CancelClicked(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
