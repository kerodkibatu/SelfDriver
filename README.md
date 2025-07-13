<a href="https://youtu.be/NqDyxk_iiCg " target="_blank">
  <img width="500" alt="Project Showcase" src="https://github.com/user-attachments/assets/cd2f9c7e-c730-46a2-bbd1-743d37b796dc " />
</a>

# Self-Driving Car Simulation with Unity ML-Agents

This project implements a self-driving car simulation environment using Unity and ML-Agents, where autonomous vehicles learn to navigate to targets while avoiding obstacles.

## Project Overview

The simulation consists of the following key components:

### 1. Driver Agent (ML-Agents)
- Uses Unity ML-Agents for reinforcement learning
- Observes:
  - Bearing to target
  - Current speed (normalized)
  - Steering angle
- Actions:
  - Throttle control (forward/reverse/off)
  - Steering control (left/right/center)
- Rewards:
  - Positive reward for reaching targets (+10)
  - Negative reward for collisions with walls (-2)
  - Small penalties for:
    - Drifting (-0.01)
    - Quick turns (based on angular velocity)
    - Time penalty (-0.001 per step)
  - Small rewards for maintaining speed

### 2. Car Controller (PROMETEO)
- Realistic vehicle physics simulation
- Configurable parameters:
  - Max speed (forward/reverse)
  - Acceleration and steering characteristics
  - Brake force and drift behavior
- Features:
  - Wheel physics with friction curves
  - Drift mechanics
  - Optional particle effects for tire smoke
  - Optional sound effects (engine, tire screech)

### 3. Training Environment
- Multiple training environments can be instantiated in a grid
- Each environment contains:
  - A car with ML-Agents driver
  - Randomly positioned targets
  - Boundary walls
- Environments reset when:
  - Target is reached (success)
  - Collision with walls (failure)

## Technical Details

### ML-Agents Configuration
- Discrete action space for both throttle and steering
- Observations include normalized values for efficient learning
- Episode management with automatic resets
- Support for both training and manual control (heuristic mode)

### Physics Setup
- Uses Unity's wheel collider system
- Configurable center of mass for realistic behavior
- Detailed friction curves for drift mechanics
- Rigidbody-based physics interactions

## Usage

1. Open the project in Unity
2. The scene contains a Manager object that can spawn multiple training environments
3. Adjust the training parameters in the ML-Agents configuration
4. Use ML-Agents CLI for training:
   ```
   mlagents-learn config/ppo/SelfDriver.yaml --run-id=training_run_1
   ```

## Project Structure

- `Assets/Scripts/`
  - `DriverAgent.cs`: ML-Agents implementation
  - `Manager.cs`: Training environment management
- `Assets/PROMETEO - Car Controller/`
  - Complete car physics implementation
- `Assets/ML-Agents/`
  - Training configurations and results

## Dependencies

- Unity 2020.3 or later
- ML-Agents 2.0 or later
- PROMETEO Car Controller asset
