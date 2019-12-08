
var EXO = function () {
    return {
        alert(message, callback) {
            Swal.fire({
                title: 'Notification',
                text: message ? message:"",
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'OK'
            }).then(typeof callback === 'function' ? callback : (result) => { })
        },
        confirm(message, callback) {
            Swal.fire({
                title: 'Are you sure?',
                text: message ? message : "",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'OK'
            }).then(typeof callback === 'function' ? callback : (result) => { });
        },
        error() {
            EXO.alert("Hệ thống đang bận! Vui lòng làm mới trình duyệt và thử lại!");
            App.stopPageLoading();
        },
        isValidEmails(email) {
            var re = /^((([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}));)*((([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,})))+$/;
            return re.test(String(email).toLowerCase());
        },
        isEmail(email) {
            var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            return re.test(String(email).toLowerCase());
        },
        isValidPhoneNumber(phone) {
            var re = /^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$/g;
            return re.test(phone);
        },
        isValidPassword(pass) {
            //Password must has digits, characters and at least 6 characters
            //let re = /[a-z]\d|\d[a-z]/i;
            let re = /^(((?=.*[a-z])(?=.*[A-Z]))|((?=.*[a-z])(?=.*[0-9]))|((?=.*[A-Z])(?=.*[0-9])))(?=.{6,})/i;
            //var strongRegex = new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,})");
            //var mediumRegex = new RegExp("^(((?=.*[a-z])(?=.*[A-Z]))|((?=.*[a-z])(?=.*[0-9]))|((?=.*[A-Z])(?=.*[0-9])))(?=.{6,})");

            return re.test(pass) && pass.length > 5;
        },
        isValidPath(path) {
            //var re = /^([a-zA-Z]:)?(\\[^<>:"\/\\|?*]+)*\\?$/;
            var re = /^([\\\/][^<>:"\/\\|?*]+)*[\\\/]?$/;
            return re.test(String(path));
        },
        isValidUserName(path) {
            //Username is 2-20 characters and digits, no _ or . at the beginning and the end
            var re = /^(?=.{2,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$/;
            return re.test(String(path));
        },
    };
}();