using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class FrmTicTacToe : Form
    {
        public FrmTicTacToe()
        {
            InitializeComponent();
        }

        private void ButtonMouseEnter(Panel buttonPanel)
        {
            buttonPanel.BackColor = Color.Purple;
            Cursor = Cursors.Hand;
        }

        private void panelCloseButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void panelCloseButton_MouseEnter(object sender, EventArgs e)
        {
            ButtonMouseEnter(panelCloseButton);
        }

        private void panelCloseButton_MouseLeave(object sender, EventArgs e)
        {
            panelCloseButton.BackColor = Color.Indigo;
            Cursor = Cursors.Default;
        }

        private void labelClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void labelClose_MouseEnter(object sender, EventArgs e)
        {
            ButtonMouseEnter(panelCloseButton);
        }
    }
}
