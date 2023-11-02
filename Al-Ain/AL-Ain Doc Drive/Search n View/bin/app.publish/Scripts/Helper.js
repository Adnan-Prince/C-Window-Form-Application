function EmailChecker(val) {
    var Issue = false;
    var pattern = /^\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b$/i;
    if (val.includes(';')) {
        val = val.split(';');
        for (var i = 0; i < val.length; i++) {
            if (val[i] !== ""){
                if (!pattern.test(val[i])) {
                    Issue = true;
                }
            }
        }
        if (!Issue)
            return true;
        else
            return false;
    } else {
        if (!pattern.test(val)) {
            return false;
        } else
            return true;
    }
}