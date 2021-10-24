# FranzosoVisuals

A flexibility centered, SFML-based library for visualizing 2D maths.
Until further notice, Program.cs is included and could be used as demo.

Everything in the library is passed around by IValue<> class
IValue<> means functions and references
meaning you can plug into any argument either a function or a value.
Reference type Rf<> is the most basic IValue<> and if you plug it into any object, and edit it's value. The object will adapt to the changes.
If you wish the value to be constant and don't care about the transformations, just create a new Rf<>

FuncA<TArgs,TResult> is also a type of IValue<>
it is a reference type, that contains a Function and a reference to an argument.
FuncA<Grid,string> for example takes a grid and outputs a string based on the function given in the constructor

the program works similliar to a powerpoint presentation.
Each window has a list of actions, on click the window performs an action.

If you have more intrest in the library, I'm happy to receive any questions :)
I won't provide any more details since I sincerely doubt more than 10 people would ever use it

pls don't judge me by the library name, I think it's cool
