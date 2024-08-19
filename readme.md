Using `FileStream` instead of `MemoryStream` looked like a good way to reduce memory consumption.
However, benchmarking showed underwhelming results.

| Method                          | AttachmentSize | Mean      | Error     | StdDev   | Ratio | RatioSD | Gen0       | Gen1      | Gen2      | Allocated | Alloc Ratio |
|-------------------------------- |--------------- |----------:|----------:|---------:|------:|--------:|-----------:|----------:|----------:|----------:|------------:|
| UsingMemoryStreamWithToArray    | 10485760       |  40.90 ms |  1.001 ms | 0.662 ms |  1.00 |    0.02 |  1166.6667 | 1166.6667 | 1166.6667 | 100.45 MB |        1.00 |
| UsingMemoryStreamWithoutToArray | 10485760       |  45.32 ms |  1.308 ms | 0.865 ms |  1.11 |    0.03 |  3333.3333 |  750.0000 |  333.3333 | 123.32 MB |        1.23 |
| UsingFileStream                 | 10485760       |  78.52 ms |  2.213 ms | 1.158 ms |  1.92 |    0.04 |  3000.0000 |  285.7143 |         - |  73.09 MB |        0.73 |
|                                 |                |           |           |          |       |         |            |           |           |           |             |
| UsingMemoryStreamWithToArray    | 20971520       |  79.16 ms |  3.927 ms | 2.598 ms |  1.00 |    0.04 |  1285.7143 | 1285.7143 | 1285.7143 | 200.87 MB |        1.00 |
| UsingMemoryStreamWithoutToArray | 20971520       |  88.91 ms |  4.387 ms | 2.295 ms |  1.12 |    0.04 |  6166.6667 | 1000.0000 |  166.6667 | 246.56 MB |        1.23 |
| UsingFileStream                 | 20971520       | 154.45 ms |  9.726 ms | 6.433 ms |  1.95 |    0.10 |  6000.0000 |  750.0000 |         - |  146.1 MB |        0.73 |
|                                 |                |           |           |          |       |         |            |           |           |           |             |
| UsingMemoryStreamWithToArray    | 31457280       | 103.03 ms |  1.893 ms | 1.252 ms |  1.00 |    0.02 |  1400.0000 | 1400.0000 | 1400.0000 | 251.05 MB |        1.00 |
| UsingMemoryStreamWithoutToArray | 31457280       | 118.12 ms |  1.510 ms | 0.898 ms |  1.15 |    0.02 |  9000.0000 | 1000.0000 |         - | 319.57 MB |        1.27 |
| UsingFileStream                 | 31457280       | 236.83 ms | 10.145 ms | 6.711 ms |  2.30 |    0.07 |  9000.0000 | 1000.0000 |         - | 219.11 MB |        0.87 |
|                                 |                |           |           |          |       |         |            |           |           |           |             |
| UsingMemoryStreamWithToArray    | 41943040       | 154.27 ms |  2.106 ms | 1.253 ms |  1.00 |    0.01 |  1500.0000 | 1500.0000 | 1500.0000 | 401.69 MB |        1.00 |
| UsingMemoryStreamWithoutToArray | 41943040       | 159.47 ms |  3.574 ms | 2.364 ms |  1.03 |    0.02 | 12000.0000 | 1333.3333 |         - | 493.05 MB |        1.23 |
| UsingFileStream                 | 41943040       | 234.57 ms | 14.543 ms | 9.619 ms |  1.52 |    0.06 | 12000.0000 | 1000.0000 |         - | 292.12 MB |        0.73 |

Only 27% reduction in memory and processing time increased by 52% will result in worse throughput.
