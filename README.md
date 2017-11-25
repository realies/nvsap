# nvsap

NVIDIA's SHIELD service requires users to pair devices by filling out a randomly generated PIN in a textfield prompt.

![SHIELD Auth Prompt](https://i.snag.gy/CM8iG1.jpg)

To ease pairing with new remote devices, this software serves as lighweight HTTP server capable of populating a PIN on demand remotely by accepting GET requests on `http://+:47980/auth/1234` where `1234` is a 4 digit PIN. Upon receiving a GET request, the software checks for an existing SHIELD auth prompt, populates the PIN, clicks the `Connect` button and responds with `true` or `false` depending on the success of the operations. Currently there is no way of validating pairing requests and it is expected that a client attempting to pair will receive a pairing status response from the SHIELD server.

![nvsap](https://i.snag.gy/Ekdf7P.jpg)
