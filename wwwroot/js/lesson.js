// Функціональність вкладок
document.querySelectorAll('.tab-btn').forEach(button => {
    button.addEventListener('click', () => {
        // Деактивувати всі вкладки
        document.querySelectorAll('.tab-btn').forEach(btn => btn.classList.remove('active'));
        document.querySelectorAll('.tab-content').forEach(content => content.classList.remove('active'));

        // Активувати обрану вкладку
        button.classList.add('active');
        document.getElementById(button.getAttribute('data-tab')).classList.add('active');

        // Оновити прогрес уроку
        updateProgress();
    });
});

// Навігаційні кнопки
document.getElementById('next-theory').addEventListener('click', () => {
    document.querySelector('[data-tab="practice"]').click();
});

document.getElementById('prev-to-theory').addEventListener('click', () => {
    document.querySelector('[data-tab="theory"]').click();
});

document.getElementById('next-to-flashcards').addEventListener('click', () => {
    document.querySelector('[data-tab="flashcards"]').click();
});

document.getElementById('prev-to-practice').addEventListener('click', () => {
    document.querySelector('[data-tab="practice"]').click();
});

document.getElementById('next-to-challenge').addEventListener('click', () => {
    document.querySelector('[data-tab="challenge"]').click();
});

document.getElementById('prev-to-flashcards').addEventListener('click', () => {
    document.querySelector('[data-tab="flashcards"]').click();
});

// Функція оновлення прогресу
function updateProgress() {
    const tabs = document.querySelectorAll('.tab-btn');
    const activeIndex = Array.from(tabs).findIndex(tab => tab.classList.contains('active'));
    const progressPercentage = ((activeIndex + 1) / tabs.length) * 100;
    document.getElementById('lesson-progress').style.width = `${progressPercentage}%`;
}

// Початкове оновлення прогресу
updateProgress();

// Інтерактивні картки прикладів
document.querySelectorAll('.example-card').forEach(card => {
    card.addEventListener('click', () => {
        card.classList.toggle('flipped');
    });
});

// Розгортання/згортання списків
document.querySelectorAll('.expandable-header').forEach(header => {
    header.addEventListener('click', () => {
        const content = header.nextElementSibling;
        const icon = header.querySelector('i');

        if (content.style.maxHeight) {
            content.style.maxHeight = null;
            icon.classList.replace('fa-chevron-up', 'fa-chevron-down');
        } else {
            content.style.maxHeight = content.scrollHeight + 'px';
            icon.classList.replace('fa-chevron-down', 'fa-chevron-up');
        }
    });
});

// Перемикач типів баз даних
document.querySelectorAll('.db-tab').forEach(tab => {
    tab.addEventListener('click', () => {
        // Деактивувати всі вкладки
        document.querySelectorAll('.db-tab').forEach(t => t.classList.remove('active'));
        document.querySelectorAll('.db-type-content').forEach(c => c.classList.remove('active'));

        // Активувати обрану вкладку
        tab.classList.add('active');
        document.getElementById(tab.getAttribute('data-dbtype')).classList.add('active');
    });
});

// Функціонал кнопки підказки
document.querySelector('.hint-btn').addEventListener('click', () => {
    const hintBox = document.getElementById('hint-box');
    hintBox.style.display = hintBox.style.display === 'none' || hintBox.style.display === '' ? 'block' : 'none';
});

// Рівні підказок
document.querySelectorAll('.hint-level').forEach(button => {
    button.addEventListener('click', () => {
        const level = button.getAttribute('data-level');
        const hintText = document.querySelector('.hint-text');

        if (level === '1') {
            hintText.textContent = "Спробуйте використати оператор SELECT для вибору полів з таблиці.";
        } else if (level === '2') {
            hintText.textContent = "Вам потрібно вибрати поле Title з таблиці Book.";
        } else if (level === '3') {
            hintText.textContent = "Правильний запит: SELECT Title FROM Book;";
        }
    });
});

// Кнопки форматування редактора коду
document.querySelector('.editor-toolbar .fa-indent').parentElement.addEventListener('click', () => {
    const queryInput = document.getElementById('query-input');
    // Покращене форматування SQL запиту
    let formatted = queryInput.value.trim();
    // Форматування ключових слів SQL великими літерами
    const keywords = ['SELECT', 'FROM', 'WHERE', 'AND', 'OR', 'ORDER BY', 'GROUP BY', 'HAVING', 'JOIN', 'LEFT JOIN', 'RIGHT JOIN', 'INNER JOIN'];
    keywords.forEach(keyword => {
        const regex = new RegExp('\\b' + keyword.replace(/\s+/g, '\\s+') + '\\b', 'gi');
        formatted = formatted.replace(regex, keyword);
    });
    // Додаємо переноси рядків для читабельності
    formatted = formatted.replace(/\s*(SELECT|FROM|WHERE|ORDER BY|GROUP BY|HAVING)\s+/gi, '\n$1 ');
    // Додаємо пробіли після ком
    formatted = formatted.replace(/\s*,\s*/g, ', ');
    queryInput.value = formatted;
});

document.querySelector('.editor-toolbar .fa-eraser').parentElement.addEventListener('click', () => {
    document.getElementById('query-input').value = '';
});

// Виконання SQL-запиту
document.getElementById('run-query').addEventListener('click', function () {
    const query = document.getElementById('query-input').value;
    const resultOutput = document.getElementById('result-output');

    fetch('/api/run-sql-query', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ query })
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                if (data.isCorrect) {
                    resultOutput.textContent = "Ваш запит правильний!";
                    document.getElementById('success-animation').style.display = 'block';
                    setTimeout(() => {
                        document.getElementById('success-animation').style.display = 'none';
                    }, 3000);
                } else {
                    resultOutput.textContent = "Ваш запит неправильний. Спробуйте ще раз.";
                }
                console.log("Результат виконання:", data.result);
            } else {
                resultOutput.textContent = `Помилка: ${data.error}`;
            }
        })
        .catch(error => {
            resultOutput.textContent = `Сталася помилка: ${error}`;
        });
});

// Функціонал флешкарток
const flashcards = document.querySelectorAll('.flashcard');
let currentCardIndex = 0;
const totalCards = flashcards.length;

document.getElementById('total-cards').textContent = totalCards;

function showCard(index) {
    flashcards.forEach(card => card.classList.remove('current-card'));
    flashcards[index].classList.add('current-card');
    document.getElementById('current-card').textContent = index + 1;

    // Оновлення стану кнопок навігації
    document.getElementById('prev-card').disabled = index === 0;
    document.getElementById('next-card').disabled = index === totalCards - 1;
}

document.getElementById('prev-card').addEventListener('click', () => {
    if (currentCardIndex > 0) {
        currentCardIndex--;
        showCard(currentCardIndex);
    }
});

document.getElementById('next-card').addEventListener('click', () => {
    if (currentCardIndex < totalCards - 1) {
        currentCardIndex++;
        showCard(currentCardIndex);
    }
});

// Обертання флешкарток
flashcards.forEach(card => {
    card.addEventListener('click', () => {
        card.querySelector('.flashcard-inner').classList.toggle('flipped');
    });
});

// Кнопки "Знаю" і "Повторити"
document.getElementById('know-it').addEventListener('click', () => {
    flashcards[currentCardIndex].classList.add('known');
    if (currentCardIndex < totalCards - 1) {
        currentCardIndex++;
        showCard(currentCardIndex);
    }
});

document.getElementById('review-later').addEventListener('click', () => {
    flashcards[currentCardIndex].classList.add('review');
    if (currentCardIndex < totalCards - 1) {
        currentCardIndex++;
        showCard(currentCardIndex);
    }
});

// Функціонал челенджа
let challengeTimer;
let timeLeft = 120; // 2 хвилини в секундах
let currentQuestion = 1;
const totalQuestions = document.querySelectorAll('.challenge-question').length;

// Показати запитання за номером
function showQuestion(questionNumber) {
    document.querySelectorAll('.challenge-question').forEach(q => {
        q.classList.remove('active');
    });
    document.querySelector(`.challenge-question[data-question="${questionNumber}"]`).classList.add('active');

    // Оновити індикатори запитань
    document.querySelectorAll('.question-dot').forEach(dot => {
        dot.classList.remove('active');
    });
    document.querySelector(`.question-dot[data-question="${questionNumber}"]`).classList.add('active');

    // Оновити стан кнопок навігації
    document.getElementById('prev-question').disabled = questionNumber === 1;
    document.getElementById('next-question').disabled = questionNumber === totalQuestions;
}

// Навігація по запитаннях
document.getElementById('prev-question').addEventListener('click', () => {
    if (currentQuestion > 1) {
        currentQuestion--;
        showQuestion(currentQuestion);
    }
});

document.getElementById('next-question').addEventListener('click', () => {
    if (currentQuestion < totalQuestions) {
        currentQuestion++;
        showQuestion(currentQuestion);
    }
});

// Індикатори запитань (точки)
document.querySelectorAll('.question-dot').forEach(dot => {
    dot.addEventListener('click', () => {
        currentQuestion = parseInt(dot.getAttribute('data-question'));
        showQuestion(currentQuestion);
    });
});

// Запуск таймера
function startChallenge() {
    timeLeft = 120;
    updateTimer();
    challengeTimer = setInterval(() => {
        timeLeft--;
        updateTimer();

        if (timeLeft <= 0) {
            finishChallenge();
        }
    }, 1000);
}

// Оновлення відображення таймера
function updateTimer() {
    const minutes = Math.floor(timeLeft / 60);
    const seconds = timeLeft % 60;
    document.getElementById('timer').textContent = `${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`;

    // Змінити колір таймера при малій кількості часу
    if (timeLeft <= 30) {
        document.getElementById('timer').classList.add('time-warning');
    }
}

// Завершення челенджа
function finishChallenge() {
    clearInterval(challengeTimer);

    // Підрахунок правильних відповідей
    let correctCount = 0;
    if (document.getElementById('q1-b').checked) correctCount++;
    if (document.getElementById('q2-a').checked) correctCount++;
    if (document.getElementById('q3-b').checked) correctCount++;

    // Показати результати
    document.getElementById('correct-answers').textContent = correctCount;
    document.getElementById('time-taken').textContent = `${Math.floor((120 - timeLeft) / 60).toString().padStart(2, '0')}:${((120 - timeLeft) % 60).toString().padStart(2, '0')}`;

    // Відображення результатів
    document.querySelector('.challenge-container').style.display = 'none';
    document.querySelector('.challenge-timer').style.display = 'none';
    document.getElementById('challenge-results').style.display = 'block';

    // Зворотній зв'язок відповідно до результатів
    let feedback = '';
    if (correctCount === totalQuestions) {
        feedback = '<p class="success-feedback">Відмінно! Ви відповіли правильно на всі питання!</p>';
    } else if (correctCount >= totalQuestions / 2) {
        feedback = '<p class="partial-feedback">Непогано! Але варто ще попрацювати над деякими темами.</p>';
    } else {
        feedback = '<p class="fail-feedback">Рекомендуємо пройти урок ще раз для кращого засвоєння матеріалу.</p>';
    }
    document.getElementById('challenge-feedback').innerHTML = feedback;
}

// Кнопка завершення челенджа
document.getElementById('challenge-submit').addEventListener('click', finishChallenge);

// Перезапуск челенджа
document.getElementById('restart-challenge').addEventListener('click', () => {
    // Скинути відповіді
    document.querySelectorAll('.challenge-options input[type="radio"]').forEach(input => {
        input.checked = false;
    });

    // Скинути стан інтерфейсу
    document.querySelector('.challenge-container').style.display = 'block';
    document.querySelector('.challenge-timer').style.display = 'flex';
    document.getElementById('challenge-results').style.display = 'none';

    // Показати перше питання
    currentQuestion = 1;
    showQuestion(currentQuestion);

    // Перезапустити таймер
    startChallenge();
});

// Ініціалізація при переході на вкладку "Челендж"
document.querySelector('[data-tab="challenge"]').addEventListener('click', () => {
    if (!challengeTimer) {
        startChallenge();
    }
});

// Завершення уроку
document.getElementById('finish-lesson').addEventListener('click', () => {
    alert('Вітаємо! Ви завершили урок "Вступ до баз даних".');
    // Тут можна додати перехід до наступного уроку або на сторінку усіх уроків
});

// Ініціалізація при завантаженні сторінки
document.addEventListener('DOMContentLoaded', () => {
    // Показати першу картку у наборі флешкарток
    showCard(0);

    // Показати перше питання у челенджі
    showQuestion(1);
});
