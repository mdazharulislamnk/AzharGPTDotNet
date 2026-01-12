import { useState, useRef, useEffect } from 'react';
import ReactMarkdown from 'react-markdown';
import rehypeHighlight from 'rehype-highlight';
import 'highlight.js/styles/atom-one-dark.css'; 
import './App.css';

function App() {
  const [input, setInput] = useState("");
  // Initial message matches your design ("Welcome to Azhar GPT")
  const [messages, setMessages] = useState([
    { sender: "bot", text: "🤖 Welcome to Azhar GPT!" }
  ]);
  const [loading, setLoading] = useState(false);
  
  // Auto-scroll reference
  const chatEndRef = useRef(null);

  // Scroll to bottom whenever messages change
  useEffect(() => {
    chatEndRef.current?.scrollIntoView({ behavior: "smooth" });
  }, [messages]);

  const sendMessage = async (e) => {
    e.preventDefault(); // Prevent form refresh
    if (!input.trim()) return;

    const userMessage = { sender: "user", text: input };
    setMessages((prev) => [...prev, userMessage]);
    setLoading(true);
    const originalInput = input;
    setInput(""); 

    try {
      // Connecting to your local .NET API
      const response = await fetch("http://localhost:5000/api/chat", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ message: originalInput }),
      });

      if (!response.ok) throw new Error("Server error");

      const data = await response.json();
      const botMessage = { sender: "bot", text: data.reply };
      setMessages((prev) => [...prev, botMessage]);

    } catch (error) {
      console.error("Error:", error);
      setMessages((prev) => [...prev, { sender: "bot", text: "Sorry, I couldn't reach the server." }]);
    } finally {
      setLoading(false);
    }
  };

  return (
    <>
      <header>Azhar GPT</header>
      
      <div className="chat-wrapper">
        <div className="chat-log">
          {messages.map((msg, index) => (
            <div key={index} className={`msg ${msg.sender}`}>
              {/* If it's the bot, render Markdown. If user, just text. */}
              {msg.sender === 'bot' ? (
                <ReactMarkdown rehypePlugins={[rehypeHighlight]}>
                  {msg.text}
                </ReactMarkdown>
              ) : (
                msg.text
              )}
            </div>
          ))}
          
          {loading && (
            <div className="msg bot">Thinking...</div>
          )}
          
          {/* Invisible element to auto-scroll to */}
          <div ref={chatEndRef} />
        </div>

        <form className="chat-form" onSubmit={sendMessage}>
          <input 
            type="text" 
            value={input} 
            onChange={(e) => setInput(e.target.value)} 
            placeholder="Type your message..."
            disabled={loading}
          />
          <button type="submit" disabled={loading}>
            {loading ? "..." : "Send"}
          </button>
        </form>
      </div>
    </>
  );
}

export default App;