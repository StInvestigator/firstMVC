
$(".confirm-action").on("click", e => {

    e.preventDefault()
    let target = $(e.target)
    let confirm = target.data("confirm-message")
    let success = target.data("success-message")
    let error = target.data("error-message")

    let body = target.data('body')
    let url = target.attr('formaction')

    console.log(body)

    Swal.fire({
        title: confirm,
        showDenyButton: false,
        showCancelButton: true,
        confirmButtonText: "Підтвердити",
        cancelButtonText: "Закрити"
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(url, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(body)
            }).then(d => d.json())
                .then(data => {
                    if (data.ok) {
                        Swal.fire(success, "", "success").then(() => {
                            window.location.reload()
                        });
                    }
                    else {
                        Swal.fire(error, "", "error");
                    }
                }).catch(err => {
                    console.log(err)
                    Swal.fire(error, "", "error");
                })
        }
    });
})