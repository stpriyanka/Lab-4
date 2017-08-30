$(function () {
	$('#error').click(function () {
		// make it not dissappear
		toastr.error("Noooo oo oo ooooo!!!", "Title", {
			"timeOut": "0",
			"extendedTImeout": "0"
		});
	});

	$('#success').click(function () {
		toastr.success("Order has been proccessed!");
	});
});
