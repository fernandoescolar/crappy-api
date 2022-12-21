# Crappy API

**Warning: This API is intentionally designed to have memory leaks, CPU exhaustion, and other issues. It should not be used in production environments. Use at your own risk.**

This API is a test API that has been designed to demonstrate and test the effects of various types of issues on a system. It is not intended for use in production environments, and using it may cause harm to the system it is running on.

## Endpoints

The Crappy API has the following endpoints:

### GET cpu/{milliseconds}

This endpoint will cause the CPU to be maxed out for the specified number of milliseconds.

### GET memory/{kilobytes}

This endpoint will allocate the specified number of kilobytes of memory and keep it allocated until the memory is manually released or the process is terminated.

### DELETE memory/{kilobytes}

This endpoint will release the specified number of kilobytes of memory that was previously allocated using the `GET memory/{kilobytes}` endpoint.

### GET deadlock

**Warning: This endpoint cause a deadlock, which may make the application or system unresponsive. Use with caution.**

This endpoint will cause the application to enter a deadlock state.

### GET stackoverflow

**Warning: This endpoint can cause a stack overflow, which may make the application or system unresponsive. Use with caution.**

This endpoint will cause a stack overflow error to occur.

## Usage

To use the Crappy API, clone the repository and run the API using the dotnet command.

```bash
git clone https://github.com/fernandoescolar/crappy-api.git
cd crappy-api
dotnet run
```

Or you can use the Docker image.

```bash
docker run -p 5000:80 ghcr.io/fernandoescolar/crappy-api:latest
```

## Testing
To safely test the Crappy API, it is recommended to use a virtual machine or a test environment with limited resources. Do not use the Crappy API on a production system or a system with important data, as it may cause harm to the system.

## Disclaimer

The Crappy API is provided as-is, without any warranties or guarantees. Use at your own risk.

## License

The Crappy API is licensed under the [MIT license](LICENSE).