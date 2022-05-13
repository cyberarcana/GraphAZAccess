# Graph AZ Access - Microsoft Sentinel

```JSON
Introduce additional capabilities in Microsoft Sentinel by using a graph approach to identify and visualize attack paths and pivot opportunities for privilege escalation in Azure.
```

## Inspiration / Short Description

Microsoft Sentinel provides a multitude of connectors to ingest security events from a wide range of sources. Drawing inspiration from the graph based approach of tools like [Stormspotter](https://github.com/Azure/Stormspotter) and [BloodHound](https://github.com/BloodHoundAD/BloodHound), traditionally used in identifying and visualizing attack paths in an organization, our team was set to introduce a similar approach in Sentinel itself; revealing relationships and pivotal points across paths to privilege escalation, a capability that could be used proactively  but also enrich security investigations.

## What it does

- A custom connector ingests data from a graph database, like neo4j. Data can be collected by any tool that works with graph databases. In our prototype, BloodHound is used, utilizing the [AzureHound](https://github.com/BloodHoundAD/BloodHound/blob/master/Collectors/AzureHound.ps1) collector to pull data of interest from an Azure tenant.
- Ingested data is used in a custom workbook, which visualizes nodes and relationships in a graph view that can be filtered as necessary.
- A Jupyter notebook assists in identifying the path to a target node, while at the same time can map security alerts and events across the revealed path to provide additional context during investigation.

## Demo

For a short demo of GraphAZAccess see our [YouTube video](https://www.youtube.com/watch?v=eufTS8_w2xA)

## How to Guide

As stated above, currently, the only supported *data source* is [BloodHound](https://github.com/BloodHoundAD/BloodHound) and more specifically its Azure part - [AzureHound](https://github.com/BloodHoundAD/AzureHound/blob/master/AzureHound.ps1).
In general you would need a Bloodhound installation with data coming from an Azure Tenant using [AzureHound](https://github.com/BloodHoundAD/AzureHound/blob/master/AzureHound.ps1).
(For more details have a look at this [YouTube video](https://www.youtube.com/watch?v=gAConW5P5uU) )

## Import Data into Microsoft Sentinel

### Prerequisites

- A Microsoft Sentinel Workspace on a *target* Azure Tenant.
- A working BloodHound installation with data from the *target* Azure tenant using using [AzureHound](https://github.com/BloodHoundAD/AzureHound/blob/master/AzureHound.ps1).

### Steps to reproduce

- Export Bloodhound's Neo4j data in a JSON Lines format using the following command:

```JavaScript
apoc.export.json.all("all.json",{useTypes:true})
```

 (find out more [here](https://neo4j.com/labs/apoc/4.1/export/json/))

- Use GraphAZConnector from [here](https://ms.com) to import data from your exported Neo4j db into your Sentinel Workspace. You will need:
  - Your Microsoft Sentinel Workspace id
  - Your Microsoft Sentinel Agent Key
  - Your Bloodhound Neo4j exported database
- Import GraphAZAccess **Workbook** into your Microsoft Sentinel (find out more [here](https://docs.microsoft.com/en-us/azure/sentinel/monitor-your-data)).
- Import GraphAZAccess **Notebook** into your Microsoft Sentinel (find out more [here](https://docs.microsoft.com/en-us/azure/sentinel/notebooks)).
