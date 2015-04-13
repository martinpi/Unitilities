# Unitilities
A bunch of helpful utilities for Unity.

### What have we here?

Here's a short overview what's here. Be aware that this is a bunch of objects that I drag from project to project. Some of them are pretty stable. Others not so much. I'd rather use them as inspiration than for anything else.

- A* implementation based on [Christoph Husse's](http://www.codeproject.com/Articles/118015/Fast-A-Star-D-Implementation-for-C) original code but greatly enhanced so that user context can be provided per search call
- Three collection types: A multimap based on Microsoft's examples, a priority queue that the A* needs and a shuffle bag because all other random is unsuited for Tetris
- A couple of economy simulation stubs (very much work in progress)
- A TextFile asset and JSON reader
- An arbitrary Graph type based on Microsoft's sample code
- Integer math and integer vectors
- A math parser based on [Yerzhan Kalzhani's](http://www.codeproject.com/Tips/381509/Math-Parser-NET-Csharp
) code
- A cleaned up version of this [Notification Center](http://wiki.unity3d.com/index.php?title=CSharpNotificationCenter)
- A neural network implementation
- Realtime sound tools and examples
- A couple of UI components for Unity 5's built-in UI system. Here you find dragable elements, drag areas, lines and multilines and other useful widgets
- A custom asset utility for wrapping custom assets
- Some helper functions to make the cloning of objects from prefabs easier


### License

All files are licensed under MIT license except if otherwise noted (SimpleJSON, SpookyDNBBeat, MathParser). Feel free to copy and clone. If you use Unitilities in your games it would be great if you'd give me a shout-out.

Contact me on twitter [@martinpi](http://twitter.com/martinpi) if you need help.