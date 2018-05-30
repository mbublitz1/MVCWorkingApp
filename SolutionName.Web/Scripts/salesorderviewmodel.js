SalesOrderViewModel = function (data) {
    //this gets a refrence to 
    var self = this;
    // This uses knockout and passes data which is passed to this function
    // empty second argument
    // reference self as the viewmodel we want to populate with properties the mapping plug in creates
    // from the properties from the server side view model
    ko.mapping.fromJS(data, {}, self);

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
            }
        });


    }
}