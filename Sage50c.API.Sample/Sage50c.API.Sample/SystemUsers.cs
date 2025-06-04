using Newtonsoft.Json;
using S50cSys22;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sage50c.API.Sample
{
    public partial class SystemUsers : UserControl
    {
        public SystemUsers()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Returns a list of system users.
        /// </summary>
        /// <returns>The list of system users.</returns>
        private static List<SystemUser> GetSystemUsers()
        {
            var systemUsers = new List<SystemUser>();

            APIEngine.SystemManager.Users.OfType<SystemUser>().ToList().ForEach(fe =>
            {
                systemUsers.Add(fe);
            });

            return systemUsers;
        }

        private void BGetSystemUsers_Click(object sender, EventArgs e)
        {
            var systemUsers = GetSystemUsers();

            TbSystemUsersList.Text = JsonConvert.SerializeObject(systemUsers, Formatting.Indented);
        }
    }
}
