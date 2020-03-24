window.queryStringHelper = {

    Initialise: function (dotnetHelper) {
        document.addEventListener('click', function (event) {

            // If the clicked element doesn't have the right selector, bail.
            if (!event.target.matches('a')) return;

            // If the target uri doesn't contain a querystring, bail.
            if (event.target.getAttribute('href').indexOf('?') === -1) return;

            // We have a link which contains a querystring, raise the event.
            dotnetHelper.invokeMethodAsync('RaiseQueryStringChanged');

        }, false);
    }

}