$(document).ready(function () {
    // ********** BRANDS MANAGEMENT **********

    // Load the list of brands into the brand list and dropdown
    function loadBrands() {
        $.get('/api/brands', function (brands) {
            $('#brandList').empty();  // Clear the existing list
            $('#brandId').empty();  // Clear the dropdown options

            brands.forEach(function (brand) {
                // For brands.html: Display the list of brands
                if ($('#brandList').length) {
                    $('#brandList').append(`
                        <li>
                            ${brand.brandName} 
                            <button onclick="editBrand(${brand.brandID})">Edit</button>
                            <button onclick="deleteBrand(${brand.brandID})">Delete</button>
                        </li>
                    `);
                }

                // For models.html and vehicles.html: Populate the brand dropdown
                $('#brandId').append(`<option value="${brand.brandID}">${brand.brandName}</option>`);
            });
        }).fail(function () {
            console.error("Failed to load brands.");
        });
    }

    // Handle form submission for adding/updating a brand (brands.html)
    $('#brandForm').submit(function (e) {
        e.preventDefault();  // Prevent default form submission

        var brandID = $(this).data('brandID');  // Get the brand ID if editing
        var brandName = $('#brandName').val();  // Get the brand name from the input

        // Determine if it's an update or create based on the presence of brandID
        if (brandID) {
            // Update existing brand
            $.ajax({
                type: 'PUT',
                url: `/api/brands/${brandID}`,
                contentType: 'application/json',
                data: JSON.stringify({ brandID: brandID, brandName: brandName }), // Include ID in payload
                success: function () {
                    loadBrands();  // Reload the brand list after updating
                    $('#brandForm')[0].reset();  // Clear the form input
                    $(this).removeData('brandID');  // Clear the stored ID
                },
                error: function () {
                    alert("Error updating brand.");
                }
            });
        } else {
            // Add new brand
            $.ajax({
                type: 'POST',
                url: '/api/brands',
                contentType: 'application/json',
                data: JSON.stringify({ brandName: brandName }),
                success: function () {
                    loadBrands();  // Reload the brand list after adding
                    $('#brandForm')[0].reset();  // Clear the form input
                },
                error: function () {
                    alert("Error adding brand.");
                }
            });
        }
    });

    // Edit brand
    window.editBrand = function (brandID) {
        $.get(`/api/brands/${brandID}`, function (brand) {
            $('#brandName').val(brand.brandName);  // Pre-fill the form
            $('#brandForm').data('brandID', brandID);  // Store the ID for updating
        });
    };

    // Delete brand
    window.deleteBrand = function (brandID) {
        if (confirm("Are you sure you want to delete this brand?")) {
            $.ajax({
                type: 'DELETE',
                url: `/api/brands/${brandID}`,
                success: function () {
                    loadBrands();  // Reload the brand list after deletion
                },
                error: function () {
                    alert("Cannot delete this brand because it has associated models.");
                }
            });
        }
    };

    // ********** MODELS MANAGEMENT **********

    // Load the list of models with their associated brands (models.html)
    function loadModels() {
        $.get('/api/models', function (models) {
            $('#modelList').empty();  // Clear the existing list

            models.forEach(function (model) {
                $('#modelList').append(`
                    <li>
                        ${model.modelName} (Brand: ${model.brand.brandName}) 
                        <button onclick="editModel(${model.modelID})">Edit</button>
                        <button onclick="deleteModel(${model.modelID})">Delete</button>
                    </li>
                `);
            });
        }).fail(function () {
            console.error("Failed to load models.");
        });
    }

    // Handle form submission for adding/updating a new model (models.html)
    $('#modelForm').submit(function (e) {
        e.preventDefault();  // Prevent default form submission

        var modelID = $(this).data('modelID'); // Get the model ID if editing
        var modelData = {
            modelName: $('#modelName').val(),
            brandID: $('#brandId').val()  // Get the selected brand ID
        };

        // Determine if it's an update or create based on the presence of modelID
        if (modelID) {
            // Update existing model
            $.ajax({
                type: 'PUT',
                url: `/api/models/${modelID}`,
                contentType: 'application/json',
                data: JSON.stringify({ modelID: modelID, modelName: modelData.modelName, brandID: modelData.brandID }), // Include ID in payload
                success: function () {
                    loadModels();  // Reload the list of models after updating
                    $('#modelForm')[0].reset();  // Clear the form input
                    $(this).removeData('modelID');  // Clear the stored ID
                },
                error: function () {
                    alert("Error updating model.");
                }
            });
        } else {
            // Add new model
            $.ajax({
                type: 'POST',
                url: '/api/models',
                contentType: 'application/json',
                data: JSON.stringify(modelData),
                success: function () {
                    loadModels();  // Reload the list of models after adding
                    $('#modelForm')[0].reset();  // Clear the form input
                },
                error: function () {
                    alert("Error adding model.");
                }
            });
        }
    });

    // Edit model
    window.editModel = function (modelID) {
        $.get(`/api/models/${modelID}`, function (model) {
            $('#modelName').val(model.modelName);  // Pre-fill the form
            $('#brandId').val(model.brandID);  // Set the selected brand
            $('#modelForm').data('modelID', modelID);  // Store the ID for updating
        });
    };

    // Delete model
    window.deleteModel = function (modelID) {
        if (confirm("Are you sure you want to delete this model?")) {
            $.ajax({
                type: 'DELETE',
                url: `/api/models/${modelID}`,
                success: function () {
                    loadModels();  // Reload the model list after deletion
                },
                error: function () {
                    alert("Cannot delete this model because it has associated vehicles.");
                }
            });
        }
    };

    // ********** VEHICLES MANAGEMENT **********

    // Load the list of vehicles with their associated brands and models (vehicles.html)
    function loadVehicles() {
        $.get('/api/vehicles', function (vehicles) {
            $('#vehicleList').empty();  // Clear the existing list

            vehicles.forEach(function (vehicle) {
                $('#vehicleList').append(`
                    <li>
                        ${vehicle.brand.brandName} ${vehicle.model.modelName} - ${vehicle.year} 
                        <button onclick="editVehicle(${vehicle.vehicleID})">Edit</button>
                        <button onclick="deleteVehicle(${vehicle.vehicleID})">Delete</button>
                    </li>
                `);
            });
        }).fail(function () {
            console.error("Failed to load vehicles.");
        });
    }

    // Handle form submission for adding a new vehicle (vehicles.html)
    $('#vehicleForm').submit(function (e) {
        e.preventDefault();  // Prevent default form submission

        var vehicleID = $(this).data('vehicleID'); // Get the vehicle ID if editing
        var vehicleData = {
            brandID: $('#brandId').val(),
            modelID: $('#modelId').val(),
            year: $('#year').val()
        };

        // Determine if it's an update or create
        if (vehicleID) {
            // Update existing vehicle
            $.ajax({
                type: 'PUT',
                url: `/api/vehicles/${vehicleID}`,
                contentType: 'application/json',
                data: JSON.stringify({ vehicleID: vehicleID, brandID: vehicleData.brandID, modelID: vehicleData.modelID, year: vehicleData.year }), // Include ID in payload
                success: function () {
                    loadVehicles();  // Reload the vehicle list after updating
                    $('#vehicleForm')[0].reset();  // Clear the form input
                    $(this).removeData('vehicleID');  // Clear the stored ID
                    $('#vehicleID').val('');  // Clear the hidden vehicle ID
                },
                error: function () {
                    alert("Error updating vehicle.");
                }
            });
        } else {
            // Add new vehicle
            $.ajax({
                type: 'POST',
                url: '/api/vehicles',
                contentType: 'application/json',
                data: JSON.stringify(vehicleData),
                success: function () {
                    loadVehicles();  // Reload the vehicle list after adding
                    $('#vehicleForm')[0].reset();  // Clear the form input
                },
                error: function () {
                    alert("Error adding vehicle.");
                }
            });
        }
    });

    // Edit vehicle
    window.editVehicle = function (vehicleID) {
        $.get(`/api/vehicles/${vehicleID}`, function (vehicle) {
            $('#year').val(vehicle.year);  // Pre-fill the form
            $('#brandId').val(vehicle.brandID);  // Set the selected brand
            $('#modelId').val(vehicle.modelID);  // Set the selected model
            $('#vehicleForm').data('vehicleID', vehicleID);  // Store the ID for updating
        });
    };

    // Delete vehicle
    window.deleteVehicle = function (vehicleID) {
        if (confirm("Are you sure you want to delete this vehicle?")) {
            $.ajax({
                type: 'DELETE',
                url: `/api/vehicles/${vehicleID}`,
                success: function () {
                    loadVehicles();  // Reload the vehicle list after deletion
                },
                error: function () {
                    alert("Error deleting vehicle.");
                }
            });
        }
    };

    // Load models dynamically when a brand is selected in the vehicle form (vehicles.html)
    $('#brandId').change(function () {
        var selectedBrandId = $(this).val();
        loadModelsByBrand(selectedBrandId);  // Load models based on the selected brand
    });

    // Load models based on the selected brand (for vehicles.html)
    function loadModelsByBrand(brandId) {
        $.get(`/api/models?brandId=${brandId}`, function (models) {
            $('#modelId').empty();  // Clear the model dropdown
            $('#modelId').append('<option value="" disabled selected>Select a model</option>');  // Placeholder option

            models.forEach(function (model) {
                $('#modelId').append(`<option value="${model.modelID}">${model.modelName}</option>`);
            });
        }).fail(function () {
            console.error("Failed to load models for the selected brand.");
        });
    }

    // ********** INITIAL LOAD **********
    // Load brands, models, and vehicles on page load
    loadBrands();
    loadModels(); // This should populate the models dropdown
    if ($('#modelList').length) {  // Load models only for models.html
        loadModels(); // Load models on page load
    }
    if ($('#vehicleList').length) {  // Load vehicles only for vehicles.html
        loadVehicles();
    }
});
