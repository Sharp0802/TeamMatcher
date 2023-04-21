# TeamMatcher

![](https://img.shields.io/tokei/lines/github/Sharp0802/TeamMatcher)
![](https://img.shields.io/github/license/sharp0802/TeamMatcher)
![](https://img.shields.io/github/release-date-pre/sharp0802/TeamMatcher)
![](https://img.shields.io/github/languages/top/sharp0802/TeamMatcher)
![](https://img.shields.io/github/languages/count/sharp0802/TeamMatcher)

Language: [English](README.md), [한국어](README.ko-KR.md)

A cross-platform stable heuristic team-matching program, written in C#.

## NAME

`teammatcher` - divide, shuffle and stable the data into the teams of specific count

## SYNOPSIS

```shell
$ teammatcher [option]...
```

## DESCRIPTION

You can use below shell command to generate team data.

```shell
$ teammatcher --csv-file <your-csv-data> --team-count <team-count>
```

The header of your CSV data should be like this:

```csv
name, kind,  num,val
name1,female,01, 06
name2,male,  02, 07
name3,female,03, 08
name4,male,  04, 09
```

or like this:

```csv
name, kind,  num,val
name1,kind-0,01, 06
name2,kind-1,02, 07
name3,kind-0,03, 08
name4,kind-2,04, 09
```

In this CSV header, each property represent these:

- `name`: The human-readable name.
- `kind`: The kind of row. Shuffling operation will be seperated by this kind. Thus, After shuffling, The count of row of specific kind in each team will be approximately uniform.
- `num`: The id of row.
- `val`: The value of row. Shuffler will use this value as ordering key.

The order of each header property doesn't affect to shuffling operation.

## OPTIONS

- `--csv-file`: A CSV file containing data
- `--team-count`: A count of result teams.

## WARNINGS

`teammatcher` uses heuristic algorithms.
Thus, A result of `teammatcher` may differ to best approximation.

As `teammatcher` uses heuristic algorithms,
Increasing the count of result teams causes increasing error of result.

## COMPILING (cli)

### PREREQUISITES

- dotnet SDK 8.0.0 or higher

### COMPILING

```shell
cd <project-dir>/src
dotnet publish -c Release -o publish
```

Then, you can run `teammatcher` with these instructions:

```shell
cd publish
teammatcher [option]...
```
