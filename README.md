# CQRS + Event Sourcing + MongoDB

Based on CQRSLITE from Gaute Magnussen https://github.com/gautema/CQRSlite.
A CQRS demo applied to the e-commerce / marketplace business domain to show:
* Command handlers, event handlers, and events organisation based on basic business rule
* Response times from a basic web site, with parallel massive writes happening at the same time (a common problem with marketplaces)

Requires a running MongoDB instance (use docker for more simplicity)

## CQRSTest

**Start mongo for tests purposes**

docker run --name mongo -d -p 27017:27017 mongo

**Connect to mongo instance in it mode**

docker run -it --link mongo:mongo --rm mongo sh -c 'exec mongo "$MONGO_PORT_27017_TCP_ADDR:$MONGO_PORT_27017_TCP_PORT/test"'

## Notes

Bloqué sur l'insertion de Events dans MongoDB
Je souhaite manipuler une Collection<IEvent>, sans succès