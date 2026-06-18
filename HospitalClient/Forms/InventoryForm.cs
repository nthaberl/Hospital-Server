using HospitalClient.HospitalManagementSystem;
using HospitalClient.Models;
using HospitalClient.Services;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HospitalClient.Forms
{
    public partial class InventoryForm : Form
    {
        private readonly SQLService _sqlService;
        private BindingList<InventoryItem> _inventoryItems;
        private HubConnection _hubConnection;

        private string CurrentRole
        {
            get
            {
                if (UserSession.CurrentUser == null || UserSession.CurrentUser.Role == null)
                {
                    return string.Empty;
                }

                return UserSession.CurrentUser.Role.ToLower();
            }
        }

        public InventoryForm()
        {
            InitializeComponent();
            _sqlService = new SQLService();
            _inventoryItems = new BindingList<InventoryItem>();

            ConfigureRoleAccess();
            LoadInventory();
            StartSignalRConnection();
        }

        private void ConfigureRoleAccess()
        {
            labelRole.Text = "Role: " + CurrentRole;

            bool isAdmin = CurrentRole == "admin";
            bool isStaffInventoryRole =
                CurrentRole == "admin" ||
                CurrentRole == "doctor" ||
                CurrentRole == "nurse";

            if (!isStaffInventoryRole)
            {
                MessageBox.Show(
                    "Inventory is only available to admin, doctor, and nurse roles.",
                    "Access Denied",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                BeginInvoke(new Action(ReturnToDashboard));
                return;
            }

            // Admin can fully manage items.
            // Doctor and nurse can view stock and update quantity only.
            textBoxName.ReadOnly = !isAdmin;
            textBoxCategory.ReadOnly = !isAdmin;
            textBoxThreshold.ReadOnly = !isAdmin;

            buttonAdd.Visible = isAdmin;
            buttonDelete.Visible = isAdmin;
            buttonUpdate.Text = isAdmin ? "Update Item" : "Update Qty";
        }

        private async void StartSignalRConnection()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5059/inventoryHub")
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On<string, int, int, string>(
                "InventoryUpdated",
                (itemName, qtyInStock, reorderThreshold, changedByRole) =>
                {
                    RunOnUiThread(() =>
                    {
                        labelStatus.Text =
                            "Inventory updated by " + changedByRole + "; refreshing...";

                        LoadInventory();
                    });
                });

            _hubConnection.On<string, int, int>(
                "LowStockAlert",
                (itemName, qtyInStock, reorderThreshold) =>
                {
                    RunOnUiThread(() =>
                    {
                        ShowLowStockMessage(
                            itemName,
                            qtyInStock,
                            reorderThreshold);

                        labelStatus.Text = "Low stock alert received";
                    });
                });

            try
            {
                await _hubConnection.StartAsync();
                labelStatus.Text = "SignalR: connected";
            }
            catch (Exception ex)
            {
                labelStatus.Text = "SignalR: offline";

                MessageBox.Show(
                    "SignalR connection failed. Make sure HospitalServer is running.\n\n" +
                    ex.Message,
                    "SignalR Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void ShowLowStockMessage(
            string itemName,
            int qtyInStock,
            int reorderThreshold)
        {
            MessageBox.Show(
                itemName + " has reached its reorder threshold.\n\n" +
                "Current stock: " + qtyInStock + "\n" +
                "Reorder threshold: " + reorderThreshold,
                "Low Stock Alert",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        private void LoadInventory()
        {
            try
            {
                _inventoryItems = new BindingList<InventoryItem>(
                    _sqlService.GetInventoryItems());

                dataGridViewInventory.DataSource = null;
                dataGridViewInventory.AutoGenerateColumns = true;
                dataGridViewInventory.DataSource = _inventoryItems;

                FormatInventoryGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not load inventory. Make sure the Inventory table exists " +
                    "and the SQL Server connection string is correct.\n\n" +
                    ex.Message,
                    "Inventory Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void FormatInventoryGrid()
        {
            dataGridViewInventory.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dataGridViewInventory.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dataGridViewInventory.MultiSelect = false;
            dataGridViewInventory.ReadOnly = true;
            dataGridViewInventory.AllowUserToAddRows = false;

            if (dataGridViewInventory.Columns.Contains("ItemId"))
            {
                dataGridViewInventory.Columns["ItemId"].HeaderText = "ID";
            }

            if (dataGridViewInventory.Columns.Contains("Name"))
            {
                dataGridViewInventory.Columns["Name"].HeaderText = "Item Name";
            }

            if (dataGridViewInventory.Columns.Contains("QtyInStock"))
            {
                dataGridViewInventory.Columns["QtyInStock"].HeaderText =
                    "Qty In Stock";
            }

            if (dataGridViewInventory.Columns.Contains("ReorderThreshold"))
            {
                dataGridViewInventory.Columns["ReorderThreshold"].HeaderText =
                    "Reorder Threshold";
            }

            if (dataGridViewInventory.Columns.Contains("IsLowStock"))
            {
                dataGridViewInventory.Columns["IsLowStock"].HeaderText =
                    "Low Stock";
            }
        }

        private InventoryItem GetSelectedItem()
        {
            if (dataGridViewInventory.CurrentRow == null)
            {
                return null;
            }

            return dataGridViewInventory.CurrentRow.DataBoundItem as InventoryItem;
        }

        private InventoryItem BuildItemFromInputs(int itemId)
        {
            int qty;
            int threshold;

            if (!int.TryParse(textBoxQty.Text, out qty) || qty < 0)
            {
                MessageBox.Show("Qty In Stock must be a whole number 0 or higher.");
                return null;
            }

            if (!int.TryParse(textBoxThreshold.Text, out threshold) || threshold < 0)
            {
                MessageBox.Show(
                    "Reorder Threshold must be a whole number 0 or higher.");
                return null;
            }

            if (textBoxName.Text.Trim().Length == 0 ||
                textBoxCategory.Text.Trim().Length == 0)
            {
                MessageBox.Show("Name and Category are required.");
                return null;
            }

            return new InventoryItem
            {
                ItemId = itemId,
                Name = textBoxName.Text.Trim(),
                Category = textBoxCategory.Text.Trim(),
                QtyInStock = qty,
                ReorderThreshold = threshold
            };
        }

        private async void buttonAdd_Click(object sender, EventArgs e)
        {
            if (CurrentRole != "admin")
            {
                MessageBox.Show("Only admin can add inventory items.");
                return;
            }

            InventoryItem item = BuildItemFromInputs(0);

            if (item == null)
            {
                return;
            }

            try
            {
                InventoryItem savedItem = _sqlService.InsertInventoryItem(item);
                bool becameLowStock = savedItem.IsLowStock;

                LoadInventory();

                if (becameLowStock)
                {
                    ShowLowStockMessage(
                        savedItem.Name,
                        savedItem.QtyInStock,
                        savedItem.ReorderThreshold);
                }

                await NotifyInventoryChanged(savedItem, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not add item.\n\n" + ex.Message);
            }
        }

        private async void buttonUpdate_Click(object sender, EventArgs e)
        {
            InventoryItem selected = GetSelectedItem();

            if (selected == null)
            {
                MessageBox.Show("Select an item first.");
                return;
            }

            bool wasLowStock = selected.IsLowStock;

            int qty;

            if (!int.TryParse(textBoxQty.Text, out qty) || qty < 0)
            {
                MessageBox.Show("Qty In Stock must be a whole number 0 or higher.");
                return;
            }

            try
            {
                InventoryItem updatedItem;

                if (CurrentRole == "admin")
                {
                    updatedItem = BuildItemFromInputs(selected.ItemId);

                    if (updatedItem == null)
                    {
                        return;
                    }

                    _sqlService.UpdateInventoryItem(updatedItem);
                }
                else
                {
                    _sqlService.UpdateInventoryQuantity(selected.ItemId, qty);
                    updatedItem =
                        _sqlService.GetInventoryItemById(selected.ItemId);
                }

                bool becameLowStock =
                    updatedItem != null &&
                    !wasLowStock &&
                    updatedItem.IsLowStock;

                LoadInventory();

                if (becameLowStock)
                {
                    ShowLowStockMessage(
                        updatedItem.Name,
                        updatedItem.QtyInStock,
                        updatedItem.ReorderThreshold);
                }

                await NotifyInventoryChanged(updatedItem, wasLowStock);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not update item.\n\n" + ex.Message);
            }
        }

        private async void buttonDelete_Click(object sender, EventArgs e)
        {
            if (CurrentRole != "admin")
            {
                MessageBox.Show("Only admin can delete inventory items.");
                return;
            }

            InventoryItem selected = GetSelectedItem();

            if (selected == null)
            {
                MessageBox.Show("Select an item first.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Delete " + selected.Name + "?",
                "Confirm Delete",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning);

            if (result != DialogResult.OK)
            {
                return;
            }

            try
            {
                _sqlService.DeleteInventoryItem(selected.ItemId);
                LoadInventory();
                await NotifyInventoryRefresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not delete item.\n\n" + ex.Message);
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            LoadInventory();
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            ReturnToDashboard();
        }

        private void ReturnToDashboard()
        {
            DashboardForm dashboard = new DashboardForm();
            dashboard.Show();
            Hide();
        }

        private void dataGridViewInventory_SelectionChanged(
            object sender,
            EventArgs e)
        {
            InventoryItem selected = GetSelectedItem();

            if (selected == null)
            {
                return;
            }

            textBoxName.Text = selected.Name;
            textBoxCategory.Text = selected.Category;
            textBoxQty.Text = selected.QtyInStock.ToString();
            textBoxThreshold.Text = selected.ReorderThreshold.ToString();
        }

        private void dataGridViewInventory_RowPrePaint(
            object sender,
            DataGridViewRowPrePaintEventArgs e)
        {
            DataGridViewRow row = dataGridViewInventory.Rows[e.RowIndex];
            InventoryItem item = row.DataBoundItem as InventoryItem;

            if (item != null && item.IsLowStock)
            {
                row.DefaultCellStyle.BackColor = Color.MistyRose;
            }
            else
            {
                row.DefaultCellStyle.BackColor = Color.White;
            }
        }

        private async Task NotifyInventoryChanged(
            InventoryItem item,
            bool wasLowStock)
        {
            if (item == null ||
                _hubConnection == null ||
                _hubConnection.State != HubConnectionState.Connected)
            {
                return;
            }

            await _hubConnection.InvokeAsync(
                "NotifyInventoryChanged",
                item.Name,
                item.QtyInStock,
                item.ReorderThreshold,
                CurrentRole,
                wasLowStock);
        }

        private async Task NotifyInventoryRefresh()
        {
            if (_hubConnection == null ||
                _hubConnection.State != HubConnectionState.Connected)
            {
                return;
            }

            await _hubConnection.InvokeAsync(
                "NotifyInventoryRefresh",
                CurrentRole);
        }

        private void RunOnUiThread(Action action)
        {
            if (IsDisposed)
            {
                return;
            }

            if (InvokeRequired)
            {
                BeginInvoke(action);
            }
            else
            {
                action();
            }
        }

        protected override async void OnFormClosing(FormClosingEventArgs e)
        {
            if (_hubConnection != null)
            {
                await _hubConnection.DisposeAsync();
            }

            base.OnFormClosing(e);
        }
    }
}
