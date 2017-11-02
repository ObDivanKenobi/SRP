using CodeSmells.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeSmells
{
    public partial class RegisterForm : Form
    {
        public User newUser { get; private set; }

        public RegisterForm()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            string newLogin = textBoxLogin.Text,
                   newPassword = textBoxPassword.Text,
                   newPasswordRepeat = textBoxRepeatPassword.Text;

            bool invalidData = false;

            if (!User.IsValidUser(newLogin))
            {
                invalidData = true;
                labelLoginError.Visible = true;
            }
            if (!User.IsValidPassword(newPassword))
            {
                invalidData = true;
                labelPasswordError.Visible = true;
            }
            if (newPassword != newPasswordRepeat)
            {
                invalidData = true;
                labelPasswordsMismatch.Visible = true;
            }

            if (invalidData)
            {
                textBoxRepeatPassword.Clear();
                return;
            }

            newUser = new User(newLogin, newPassword);
            if (!newUser.Save())
            {
                MessageBox.Show("Такое имя пользователя уже используется!", "Ошибка регистрации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Вы успешно зарегистрированы. Используйте имя пользователя и пароль, чтобы войти в систему.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
        }

        private void textBoxLogin_TextChanged(object sender, EventArgs e)
        {
            labelLoginError.Visible = false;
        }

        private void labelPasswordError_TextChanged(object sender, EventArgs e)
        {
            labelPasswordError.Visible = false;
            labelPasswordsMismatch.Visible = false;
        }

        private void labelPasswordsMismatch_TextChanged(object sender, EventArgs e)
        {
            labelPasswordsMismatch.Visible = false;
        }
    }
}
