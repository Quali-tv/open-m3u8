## How can I contribute?

* create issues for bugs you find
* create pull requests to fix bigs, add features, clean up code, etc.
* improve the documentation

## What can I work on?

The library does not yet support the full m3u8 specification! So a great way to help is by adding support for more tags in the specification:

http://tools.ietf.org/html/draft-pantos-http-live-streaming-14

The issues page is another good place to look for ways to contribute.

## Code Style
carried forward (for now) from the iHeartRadio Java implementation

* 4 spaces per indent - no tab characters
* mPrefix private members
* opening braces on the same line
* when wrapping long method calls, put each argument on its own line
* when building long fluent builders, put each method in the chain on its own line

## Working with the Code

### Visibility

Everything not meant to be visible to the public API must be protected. If a whole class is protected, then you may mark the fields public since they will still not be visible.

### Data classes

The data structures in the data directory reflect the structure of a playlist based on the specification. They are part of the public API and must be immutable. The `Playlist` is the result of parsing and the input of writing.
