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
        if (self.ObjectState() != ObjectState.Added) {
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
SalesOrderViewModel = function (data) {
    //this is a refrence to prototype object (model class constructor)
    var self = this;
    // This uses knockout and passes data which is passed to this function
    // empty second argument
    // reference self as the viewmodel we want to populate with properties the mapping plug in creates
    // from the properties from the server side view model
    ko.mapping.fromJS(data, salesOrderItemMapping, self);

    //This will be the function used but the save button in the form
    //it will be wired up using data-bind of the button
    self.save = function (e) {
        $.ajax({
            url: "/Sales/Save/",
            type: "POST",
            //Converts data to Json using knockout built in function
            data: ko.toJSON(self),
            contentType: "application/json",
            success: function (data) {
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
        self.flagSalesOrderAsEdited = function () {
            if (self.ObjectState() !== ObjectState.Added) {

                //Remember that knockout creates observables which are functions not properties
                //so setting their values is done by passing the values to the function
                self.ObjectState(ObjectState.Modified);
            }
            // To tell knockout you want to allow the default action on the control that raised this event you
            // need to return true
            return true;
        },
        self.addSalesOrderItem = function () {
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
        self.Total = ko.computed(function () {
            var total = 0;
            ko.utils.arrayForEach(self.SalesOrderItems(),
                function (salesOrderItem) {
                    total += parseFloat(salesOrderItem.ExtendedPrice());
                });

            return total.toFixed(2);
        }),
        self.deleteSalesOrderItem = function (salesOrderItem) {
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
            required: true,
            maxlength: 30
        },
        PONumber: {
            maxlength: 10
        },
        ProductCode: {
            required: true,
            maxlength: 15
        },
        Quantity: {
            required: true,
            digits: true,
            range: [1, 1000000]
        },
        UnitPrice: {
            required: true,
            number: true,
            range: [0, 100000]

        }
    },
    messages: {
        CustomerName: {
            required: "You cannot create a sales order unless you specify the customers name.",
            maxlength: "Customer names must be 30 characters or shorter."
        }
    }
    //showErrors: function (errorMap, errorList) {

    //    $.each(this.successList, function (index, value) {
    //        $(value).popover('hide');
    //    });


    //    $.each(errorList, function (index, value) {

    //        console.log(value.message);

    //        var _popover = $(value.element).popover({
    //            trigger: 'manual',
    //            placement: 'top',
    //            content: value.message,
    //            template: '<div class="popover"><div class="arrow"></div><div class="popover-inner"><div class="popover-content"><p></p></div></div></div>'
    //        });

    //        _popover.data('popover').options.content = value.message;

    //        $(value.element).popover('show');

    //    });


    //}
});

//This adds a custom validation rule to jQuery validation
// that check for alpha characters only.
//$.validator.addMethod("alphaonly",
//    function (value) {
//        return /^[A-Za-z]+$/.test(value);
//    }
//);

