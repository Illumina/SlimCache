# SlimCache

SlimCache is standalone tool that creates a new set of Nirvana cache files containing only transcripts from the desired transcript source.

## Building

Remember to clone the git repo using the recursive option - this will automatically handle the Nirvana submodule:

```
git clone --recursive https://github.com/Illumina/SlimCache.git
```

After that, build the solution using the [.NET 5.0 SDK](https://dotnet.microsoft.com/download/dotnet/5.0):

```
cd SlimCache
dotnet build -c Release
```
## Prerequisites
Using the paths to your Nirvana 3.9 or later cache and reference files, use the following command to create a new set of cache files:

## Creating RefSeq-Only Cache Files
```
USAGE: SlimCache.dll <cache prefix> <reference path> <output cache prefix> <desired transcript source>
```

```bash
dotnet SlimCache.dll \
    Cache/GRCh37/Both \
    References/Homo_sapiens.GRCh37.Nirvana.dat \
    Cache/GRCh37/RefSeq \
    RefSeq
```

When this command is run, the following output is displayed:

```bash
- loading reference sequence... finished.
- loading cache... finished.
- filtering transcripts... 196,520 transcripts removed.
- removing duplicate predictions... 101,180 predictions removed.
- writing SIFT prediction cache... finished.
- writing PolyPhen prediction cache... finished.
- writing transcript cache... finished.

Peak memory usage: 1.407 GB
```

## Creating Ensembl-Only Cache Files

```bash
dotnet SlimCache.dll \
    Cache/GRCh37/Both \
    References/Homo_sapiens.GRCh37.Nirvana.dat \
    Cache/GRCh37/Ensembl \
    Ensembl
```

When this command is run, the following output is displayed:

```bash
- loading reference sequence... finished.
- loading cache... finished.
- filtering transcripts... 64,624 transcripts removed.
- removing duplicate predictions... 3,438 predictions removed.
- writing SIFT prediction cache... finished.
- writing PolyPhen prediction cache... finished.
- writing transcript cache... finished.

Peak memory usage: 1.927 GB
```