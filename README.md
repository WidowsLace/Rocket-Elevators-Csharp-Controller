# Rocket-Elevators-Csharp-Controller
The purpose of this code is to make a simple, yet effective controller for an elevator. enjoy (:

Imagine it, youre in a fancy building and you want to go to the top. Thrilling. You, the user, go up to the floor request buttons and press one. Whatever your choice was, the column will control the elevator to go to your location, and to add it to a queue. The doors open, you step inside. The column then controlls the elevator to go in the direction you requested. You start going in that direction and soon, the elevator is set to "stopped". The doors open for you, and then they close behind you as you step out. You just rode the best elevator in existence. youre welcome.

Column and elevator selection is based on a scoring system. Elevator moves and stops based on column control. Elevator is able to have multiple queues. Elevator moves at incredible speeds.
### Installation

As long as you have **.NET 6.0** installed on your computer, nothing more needs to be installed:

The code to run the scenarios is included in the Commercial_Controller folder, and can be executed there with:

`dotnet run <SCENARIO-NUMBER>`

### Running the tests

To launch the tests, make sure to be at the root of the repository and run:

`dotnet test`

With a fully completed project, you should get an output like:

![Screenshot from 2021-06-15 17-31-02](https://user-images.githubusercontent.com/28630658/122128889-3edfa500-ce03-11eb-97d0-df0cc6a79fed.png)

You can also get more details about each test by adding the `-v n` flag: 

`dotnet test -v n` 

which should give something like: 

![Screenshot from 2021-06-15 18-00-52](https://user-images.githubusercontent.com/28630658/122129140-a8f84a00-ce03-11eb-8807-33d7eab8c387.png)

# Rocket-Elevators-Csharp-Controller
