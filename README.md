# Simple.ShoppingCart
 a simple shopping cart with the goal to integrate with paymob api services.
Features :-
list products
add product to cart
checkout page
do payment using paymob

NOTE :-
For the call back to work on local host please use https://ngrok.com/ to start public server 
use this command to set the application port in ngrok cmd ==>> ngrok http [port] -host-header="localhost:[port]"
use the forwarding url in your dashboard => integrations => edit callback 
must look sonething like this https://567b09a54e9b.ngrok.io/Checkout/PaymentCallback
