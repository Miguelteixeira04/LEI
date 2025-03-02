class Question {
    constructor(questionText, options, selectOptionCallback) {
        this.questionText = questionText;
        this.options = options;
        this.selectOptionCallback = selectOptionCallback;
        this.element = null;
        this.selectedOptionIndex = 0; // Default selected option index - make it show immediately
        this.resolveFunction = null; // Function to resolve the promise
        this.time_question = 0; // Time spent on question
        this.startTime = null; // Start time when question is created
    }

    createQuestion() {
        return new Promise((resolve) => {
            this.resolveFunction = resolve; // Store the resolve function for later use

            // Record the start time
            this.startTime = new Date();

            const questionContainer = document.createElement('div');
            questionContainer.className = 'question-container';

            const questionTextElement = document.createElement('div');
            questionTextElement.className = 'question-text';
            questionTextElement.innerText = this.questionText;

            const optionsContainer = document.createElement('div');
            optionsContainer.className = 'options-container';

            this.options.forEach((optionText, index) => {
                const optionElement = document.createElement('div');
                optionElement.className = 'option';
                optionElement.innerText = optionText;
                optionElement.addEventListener('click', () => {
                    this.selectedOptionIndex = index;
                    this.calculateTimeSpent();
                    this.selectOptionCallback(index);
                    this.cleanup(); // Cleanup after option selection
                });
                optionsContainer.appendChild(optionElement);
            });

            questionContainer.appendChild(questionTextElement);
            questionContainer.appendChild(optionsContainer);

            const mapa = document.getElementById('mapa');
            mapa.appendChild(questionContainer);
            this.element = questionContainer;

            // Show black background
            this.showBlackBackground();

            // Keyboard navigation
            document.addEventListener('keydown', this.handleKeyDown);
        });
    }

    handleKeyDown = (event) => {
        if (event.key === 'ArrowUp' && this.selectedOptionIndex > 0) {
            this.selectedOptionIndex--;
            this.updateSelectedOption();
        } else if (
            event.key === 'ArrowDown' &&
            this.selectedOptionIndex < this.options.length - 1
        ) {
            this.selectedOptionIndex++;
            this.updateSelectedOption();
        } else if (event.key === ' ') {
            this.calculateTimeSpent();
            this.selectOptionCallback(this.selectedOptionIndex);
            this.cleanup(); // Cleanup after option selection
        }
    };

    updateSelectedOption() {
        const optionElements = this.element.querySelectorAll('.option');
        optionElements.forEach((optionElement, index) => {
            if (index === this.selectedOptionIndex) {
                optionElement.classList.add('selected');
            } else {
                optionElement.classList.remove('selected');
            }
        });
    }

    calculateTimeSpent() {
        const endTime = Date.now();
        this.time_question = endTime - this.startTime;
    
        // Converter o tempo de milissegundos para minutos e segundos
        const seconds = Math.floor(this.time_question / 1000);
        const minutes = Math.floor(seconds / 60);
        const remainingSeconds = seconds % 60;
    
        // Formatar o tempo no estilo 0:00
        const formattedTime = `${minutes}:${remainingSeconds.toString().padStart(2, '0')}`;
    
        // Atualizar o conteúdo da div com id "questionTimeDisplay"
        const timerElement = document.getElementById('questionTimeDisplay');
        if (timerElement) {
            timerElement.textContent = `Time Last Question: ${formattedTime}`;
        } else {
            console.warn("Elemento #questionTimeDisplay não encontrado no DOM!");
        }
    
        // Log para depuração no console
        console.log(`Time spent on question: ${formattedTime}`);
    }
    
    showBlackBackground() {
        const blackScreen = document.getElementById('blackScreen');
        if (blackScreen) {
            blackScreen.classList.remove('hidden');
            blackScreen.style.opacity = '1';
        }
    }

    hideBlackBackground() {
        const blackScreen = document.getElementById('blackScreen');
        if (blackScreen) {
            blackScreen.classList.add('hidden');
            blackScreen.style.opacity = '0';
        }
    }

    cleanup() {
        // Remove event listener for keydown
        document.removeEventListener('keydown', this.handleKeyDown);
        // Remove the question container from the DOM
        this.element.remove();
        // Hide black background
        this.hideBlackBackground();
        // Resolve the promise with the selected option index
        if (this.resolveFunction) {
            this.resolveFunction(this.selectedOptionIndex);
        }
    }
}