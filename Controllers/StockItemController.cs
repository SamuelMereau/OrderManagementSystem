using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using DomainClasses;

namespace Controllers
{
    public class StockItemController
    {
        /// <summary>
        /// The Data Access Layer
        /// </summary>
        private Repository _repo;

        /// <summary>
        /// Instantiates the Repository object
        /// </summary>
        public StockItemController()
        {
            _repo = new Repository();
        }

        /// <summary>
        /// Gets all stock items
        /// </summary>
        /// <returns>List of stock items</returns>
        public IEnumerable<StockItem> GetStockItems()
        {
            try
            {
                return _repo.GetStockItems();
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets a stock item from an ID
        /// </summary>
        /// <param name="stockItemId"></param>
        /// <returns>Retrieved stock item object</returns>
        public StockItem GetStockItem(int stockItemId)
        {
            try
            {
                return _repo.GetStockItem(stockItemId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Updates the quantity of a stock item
        /// </summary>
        /// <param name="order"></param>
        /// <param name="stockItemId"></param>
        public void UpdateStockItemAmount(Order order, int stockItemId)
        {
            try
            {
                _repo.UpdateStockItemAmount(order, stockItemId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}