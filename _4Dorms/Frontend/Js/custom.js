(function () {
	'use strict';

	var tinyslider = function () {
		var el = document.querySelectorAll('.testimonial-slider');

		if (el.length > 0) {
			var slider = tns({
				container: '.testimonial-slider',
				items: 1,
				axis: "horizontal",
				controlsContainer: "#testimonial-nav",
				swipeAngle: false,
				speed: 700,
				nav: true,
				controls: true,
				autoplay: true,
				autoplayHoverPause: true,
				autoplayTimeout: 3500,
				autoplayButtonOutput: false
			});
		}
	};
	tinyslider();




	var sitePlusMinus = function () {

		var value,
			quantity = document.getElementsByClassName('quantity-container');

		function createBindings(quantityContainer) {
			var quantityAmount = quantityContainer.getElementsByClassName('quantity-amount')[0];
			var increase = quantityContainer.getElementsByClassName('increase')[0];
			var decrease = quantityContainer.getElementsByClassName('decrease')[0];
			increase.addEventListener('click', function (e) { increaseValue(e, quantityAmount); });
			decrease.addEventListener('click', function (e) { decreaseValue(e, quantityAmount); });
		}

		function init() {
			for (var i = 0; i < quantity.length; i++) {
				createBindings(quantity[i]);
			}
		};

		function increaseValue(event, quantityAmount) {
			value = parseInt(quantityAmount.value, 10);

			console.log(quantityAmount, quantityAmount.value);

			value = isNaN(value) ? 0 : value;
			value++;
			quantityAmount.value = value;
		}

		function decreaseValue(event, quantityAmount) {
			value = parseInt(quantityAmount.value, 10);

			value = isNaN(value) ? 0 : value;
			if (value > 0) value--;

			quantityAmount.value = value;
		}

		init();

	};
	sitePlusMinus();


})()

// Function to add a review to the review list
function addReview(name, content, rating) {
	var reviewList = document.getElementById('reviewList');
	var reviewItem = document.createElement('div');
	reviewItem.classList.add('review-item');

	// Convert rating value into star representation
	var stars = '';
	for (var i = 0; i < rating; i++) {
		stars += '&#9733;'; // Unicode for filled star
	}

	reviewItem.innerHTML = `
        <h4>${name}</h4>
        <p>${content}</p>
        <p>Rating: ${stars}</p>
    `;

	reviewList.appendChild(reviewItem);
}

// Add fake reviews
addReview('Moha', 'Great place to stay!', 5);
addReview('Tala', 'Average experience.', 3);
addReview('Doha', 'Not recommended.', 2);

document.getElementById('reviewForm').addEventListener('submit', function (event) {
	event.preventDefault(); // Prevent the default form submission behavior

	// Get the values from the form fields
	var name = document.getElementById('reviewName').value;
	var content = document.getElementById('reviewContent').value;
	var rating = document.querySelector('input[name="rating"]:checked').value;

	// Call the addReview function to add the review to the review list
	addReview(name, content, rating);

	// Reset the form fields after submission
	document.getElementById('reviewForm').reset();
});

// Function to check availability
function checkAvailability(roomType) {
	// Dummy logic to check availability
	var isAvailable = Math.random() < 0.5; // Assume 50% chance of availability for demonstration purposes

	// Display message box with availability status
	var messageBox = document.getElementById('messageBox');
	var message = document.getElementById('message');
	var bookButton = document.getElementById('bookButton');

	if (isAvailable) {
		message.innerText = `The ${roomType} room is available!`;
		bookButton.style.display = 'block';
		bookButton.addEventListener('click', showBookingOverlay); // Add event listener to show booking overlay
	} else {
		message.innerText = `Sorry, the ${roomType} room is not available.`;
		bookButton.style.display = 'none';
	}

	messageBox.style.display = 'block';
}
// Function to close the message box
function closeMessageBox() {
	var messageBox = document.getElementById('messageBox');
	messageBox.style.display = 'none';
}

// Function to show the booking overlay
function showBookingOverlay() {
	var bookingOverlay = document.getElementById('bookingOverlay');
	bookingOverlay.style.display = 'block';
}

// Function to hide the booking overlay
function hideBookingOverlay() {
	var bookingOverlay = document.getElementById('bookingOverlay');
	bookingOverlay.style.display = 'none';
}
// Example function to trigger showing the booking overlay
function showBookingPage() {
	showBookingOverlay();
}
// Function to close the booking overlay
function closeBookingOverlay() {
	var bookingOverlay = document.getElementById('bookingOverlay');
	bookingOverlay.style.display = 'none';
}

// Function to calculate the Price 
function calculatePrice() {
	var duration = parseInt(document.getElementById('duration').value);
	var roomType = document.getElementById('roomType').value;
	var pricePerMonth = 500; // Example price per month

	var totalPrice = duration * pricePerMonth; // Calculate total price

	if (roomType === 'private') {
		totalPrice *= 1.5; // Increase price for private room
	}

	document.getElementById('price').value = totalPrice.toFixed(2); // Display total price
}

function closeBookingOverlay() {
	var bookingOverlay = document.getElementById('bookingOverlay');
	bookingOverlay.style.display = 'none';
}


// Function to add fake dorm in everestdorm
function addFakeDorm() {
	// Dummy data for the fake dorm
	const fakeDormData = {
		name: "Fake Dorm",
		for: "Students",
		city: "City Name",
		location: "Fake Address, City",
		universities: ["University 1", "University 2", "University 3"],
		phone: "+962-799-425-321",
		email: "fake.dorm@example.com",
		price_6: "1000",
		price_12: "2000",
		description: "Great place to stay!.",
	};

	// Update the HTML elements with fake dorm data
	document.getElementById("name").innerHTML = `<span>${fakeDormData.name}</span>`;
	document.getElementById("for").innerHTML = `<span>For:</span> ${fakeDormData.for}`;
	document.getElementById("city").innerHTML = `<span>City:</span> ${fakeDormData.city}`;
	document.getElementById("location").innerHTML = `<span>Location:</span> ${fakeDormData.location}`;
	document.getElementById("uni").innerHTML = `<span>Nearby universities:</span><br>${fakeDormData.universities.map(uni => `${uni}`).join('<br>')}`;
	document.getElementById("phone").innerHTML = `<span>Phone:</span> ${fakeDormData.phone}`;
	document.getElementById("email").innerHTML = `<span>Email:</span> <a href="mailto:${fakeDormData.email}">${fakeDormData.email}</a>`;
	document.getElementById("price_6").innerHTML = `<span>Price/6Months:</span> ${fakeDormData.price_6}`;
	document.getElementById("price_12").innerHTML = `<span>Price/12Months:</span> ${fakeDormData.price_12}`;
	document.getElementById("description").innerText = fakeDormData.description;
}

// Call the function to add fake dorm
addFakeDorm();

// Function to open payment overlay
function openPaymentOverlay() {
	document.getElementById("paymentOverlay").style.display = "block";
}

// Function to close payment overlay
function closePaymentOverlay() {
	document.getElementById("paymentOverlay").style.display = "none";
}

// Function to calculate amount to pay
function calculateAmountToPay() {
	// Retrieve the dorm price from the booking form
	var dormPrice = parseFloat(document.getElementById("price").value);

	// Calculate 10% of the dorm price
	var amountToPay = dormPrice * 0.1;

	// Update the "Amount to Pay" input field
	document.getElementById("amount").value = amountToPay.toFixed(2);
}

// Add event listener to the booking form inputs to recalculate amount when changed
document.getElementById("duration").addEventListener("change", calculateAmountToPay);
document.getElementById("roomType").addEventListener("change", calculateAmountToPay);

// Add event listener to the "Book Now" button to open the payment overlay and calculate the amount
document.querySelector("#bookingForm button[type='submit']").addEventListener("click", function (event) {
	event.preventDefault(); // Prevent the default form submission
	calculateAmountToPay(); // Calculate the amount to pay
	openPaymentOverlay(); // Open the payment overlay
});

