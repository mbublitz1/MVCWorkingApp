var ObjectState = {
    Unchanged: 0,
    Added: 1,
    Modified: 2,
    Deleted: 3
};

var salesOrderItemMapping = {
    'SalesOrderItems': {
        key: function (salesOrderItem) {
            return ko.utils.unwrapObservable(salesOrderItem.SalesOrderItemId);
        },
        create: function (options) {
            return new SalesOrderItemViewModel(options.data);
        }
    }
};

//View model declaration for SalesOrderItem
SalesOrderItemViewModel = function (data) {
    //this is a refrence to prototype object (model class constructor)
    var self = this;
    ko.mapping.fromJS(data, salesOrderItemMapping, self);

    self.flagSalesOrderItemAsEdited = function () {
        if (self.objectState() != ObjectState.Added) {
            self.ObjectState(ObjectState.Added);
        }
        return true;
    },
    //Adds a calculated column
        self.ExtendedPrice = ko.computed(function () {
            return (self.Quantity() * self.UnitPrice()).toFixed(2);
        });
};


//View model declaration for SalesOrder
SalesOrderViewModel = function(data) {
    //this is a refrence to prototype object (model class constructor)
    var self = this;
    // This uses knockout and passes data which is passed to this function
    // empty second argument
    // reference self as the viewmodel we want to populate with properties the mapping plug in creates
    // from the properties from the server side view model
    ko.mapping.fromJS(data, salesOrderItemMapping, self);

    //This will be the function used but the save button in the form
    //it will be wired up using data-bind of the button
    self.save = function() {
            $.ajax({
                url: "/Sales/Save/",
                type: "POST",
                //Converts data to Json using knockout built in function
                data: ko.toJSON(self),
                contentType: "application/json",
                success: function(data) {
                    //here we are getting an object back in data containing a viewModel
                    //and you have to navigate into the data model to access it but it will only
                    //be mapped if it is present which is checked in the if statement
                    if (data.salesOrderViewModel != null) {
                        ko.mapping.fromJS(data.salesOrderViewModel, {}, self);
                    }

                    if (data.newLocation != null) {
                        window.location = data.newLocation;
                    }

                }
            });
        },
        //This function will need to be bound to the input elements in the Create and Edit Views
        self.flagSalesOrderAsEdited = function() {
            if (self.ObjectState() !== ObjectState.Added) {

                //Remember that knockout creates observables which are functions not properties
                //so setting their values is done by passing the values to the function
                self.ObjectState(ObjectState.Modified);
            }
            // To tell knockout you want to allow the default action on the control that raised this event you
            // need to return true
            return true;
        },
        self.addSalesOrderItem = function() {
            var salesOrderItem =
                new SalesOrderItemViewModel({
                    SalesOrderItemId: 0,
                    ProductCode: "",
                    Quantity: 1,
                    UnitPrice: 0,
                    ObjectState: ObjectState.Added
                });
            self.SalesOrderItems.push(salesOrderItem);
        },
        self.Total = ko.computed(function() {
            var total = 0;
            ko.utils.arrayForEach(self.SalesOrderItems(),
                function(salesOrderItem) {
                    total += parseFloat(salesOrderItem.ExtendedPrice());
                });

            return total.toFixed(2);
        }),
        self.deleteSalesOrderItem = function(salesOrderItem) {
            self.SalesOrderItems.remove(this);

            if (salesOrderItem.SalesOrderItemId() > 0 &&
                self.SalesOrderItemsToDelete.indexOf(salesOrderItem.SalesOrderItemId()) == -1)
                self.SalesOrderItemsToDelete.push(salesOrderItem.SalesOrderItemId());
        };
};

$("form").validate({
    submitHandler: function() {
        salesOrderViewModel.save();
    },
    rules: {
        CustomerName: {
            required: true
        }
    }
});