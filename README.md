# Game.FSM
Event-driven Hierarchical Finite State Machine implementation with state nesting support

## Features

- State pattern (static hierarchy by state class inheritance).
- Nested states (dynamic hierarchy) - a branch of states in a state hierarchy tree can be active at the same time.
- Event-driven - states only handle events they care about via IEventHandler<T>.
- States can be isolated from transition logic via ITrigger for full code reuse.
- Transition entity provides convenient way to initiate transitions with support for nested target states.
- Transitions automatically handle the data transfer between the previous state and the next.
