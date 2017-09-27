# CQRS + Event Sourcing + MongoDB

Based on CQRSLITE from Gaute Magnussen https://github.com/gautema/CQRSlite.
A CQRS demo applied to the e-commerce / marketplace business domain to show:
* Command handlers, event handlers, and events organisation based on basic business rule
* Response times from a basic web site, with parallel massive writes happening at the same time (a common problem with marketplaces)

Requires a running MongoDB instance (use docker for more simplicity)

## CQRSTest

**Start mongo**

docker run --name mongo -d -p 27017:27017 mongo

**Connect to mongo instance in it mode**

docker run -it --link mongo:mongo --rm mongo sh -c 'exec mongo "$MONGO_PORT_27017_TCP_ADDR:$MONGO_PORT_27017_TCP_PORT/test"'

show dbs
use marketplacecatalog
db.events.find()
db.events.count()

## Performance results

TODO

## Notes

Suite à la migration vers csproj, le Framework compile (avec dotnet build --framework netstandard1.5)
CQRSCode compile également
Reste Test et Web

Faire un schéma de l'architecture

Ajout d'un produit via le web
Ajouter bootstrap

J'ai corrigé le bug de scan des IEvent, je ne parviens toujojurs pas à push une command dans le contexte de l'app web
Il y'a en fait un crash à l'instantiation du Mongo EventStore System.InvalidOperationException: Unable to resolve service for type 'System.Collections.Generic.IList`1[System.Type]' while attempting to activate 'CQRSCode.WriteModel.EventStore.Mongo.EventStore'