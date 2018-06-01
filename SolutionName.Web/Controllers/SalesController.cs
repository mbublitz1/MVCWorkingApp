using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SolutionName.DataLayer;
using SolutionName.Model;
using SolutionName.Web.ViewModels;

namespace SolutionName.Web.Controllers
{
    public class SalesController : Controller
    {
        private SalesContext _salesContext;

        public SalesController()
        {
            _salesContext = new SalesContext();
        }

        // GET: Sales
        public ActionResult Index()
        {
            return View(_salesContext.SalesOrders.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesOrder salesOrder = _salesContext.SalesOrders.Find(id);
            if (salesOrder == null)
            {
                return HttpNotFound();
            }

            var viewModel = new SalesOrderViewModel
            {
                SalesOrderId = salesOrder.SalesOrderId,
                CustomerName = salesOrder.CustomerName,
                PONumber = salesOrder.PONumber,
                MessageToClient = "I originated from the viewmodel, rather that the model."
            };


            return View(viewModel);
        }

        public ActionResult Create()
        {
            var viewModel = new SalesOrderViewModel();
            viewModel.ObjectState = ObjectState.Added;
            return View(viewModel);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesOrder salesOrder = _salesContext.SalesOrders.Find(id);
            if (salesOrder == null)
            {
                return HttpNotFound();
            }

            var viewModel = new SalesOrderViewModel
            {
                SalesOrderId = salesOrder.SalesOrderId,
                CustomerName = salesOrder.CustomerName,
                PONumber = salesOrder.PONumber,
                MessageToClient = string.Format("The original value of the Customer Name is {0}", salesOrder.CustomerName),
                ObjectState = ObjectState.Unchanged
            };

            return View(viewModel);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesOrder salesOrder = _salesContext.SalesOrders.Find(id);
            if (salesOrder == null)
            {
                return HttpNotFound();
            }

            var viewModel = new SalesOrderViewModel
            {
                SalesOrderId = salesOrder.SalesOrderId,
                CustomerName = salesOrder.CustomerName,
                PONumber = salesOrder.PONumber,
                MessageToClient = string.Format("You are about to delete this sales order"),
                ObjectState = ObjectState.Deleted
            };

            return View(viewModel);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _salesContext.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult Save(SalesOrderViewModel salesOrderViewModel)
        {
            SalesOrder salesOrder = new SalesOrder
            {
                SalesOrderId = salesOrderViewModel.SalesOrderId,
                CustomerName = salesOrderViewModel.CustomerName,
                PONumber = salesOrderViewModel.PONumber,
                ObjectState = salesOrderViewModel.ObjectState
            };

            //instead of hard coding salesOrder as an add, attach it instead
            _salesContext.SalesOrders.Attach(salesOrder);
            //Then you want to tell the change tracker to set the state that was returned from the helper method
            //created in the SolutionName.Model
            _salesContext.ChangeTracker.Entries<IObjectWithState>().Single().State =
                Helpers.ConvertState(salesOrder.ObjectState);
            _salesContext.SaveChanges();

            //Since when an item is deleted we don't want to return a aview the
            //Delete will return a anonymous Json object that will return a single property called new laoction
            //whose value is the controller action for displaying the list of sales orders.
            if (salesOrder.ObjectState == ObjectState.Deleted)
            {
                //This will be returned to the view we don't want to stay on we we need a way for the view 
                //to redirect to the sales order list which is /Sales/Index.  See line 34 in salesorderviewmodel.js
                return Json(new {newLocation = "/Sales/Index/"});
            }


            switch (salesOrderViewModel.ObjectState)
            {
                case ObjectState.Added:
                    salesOrderViewModel.MessageToClient = string.Format("A sales order for {0} has been added to the database.", salesOrder.CustomerName);
                    break;
                case ObjectState.Modified:
                    salesOrderViewModel.MessageToClient = string.Format("The customer name for this sals order has been updated to {0} in the database", salesOrder.CustomerName);
                    break;

            }

            //If a record is inserted the context is synced with the ID but that is never communicated back to the client
            //so the SalesOrderId needs to be set with the value returned from the DB
            salesOrderViewModel.SalesOrderId = salesOrder.SalesOrderId;
            salesOrderViewModel.ObjectState = ObjectState.Unchanged;
        
            //to make the Json object as flexible enough for all our needs we will send back an annoymous object
            //that contains whatever we need to send to the client and then let the client insepct the contents to 
            //determine what to do with it
            return Json(new {salesOrderViewModel});
        }
    }
}
